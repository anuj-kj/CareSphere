using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using CareSphere.Data.Configurations;
using CareSphere.Data.Core.DataContexts;
using CareSphere.Domains.Core;
using CareSphere.Domains.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Dac;
using NUnit.Framework;
using Testcontainers.MsSql;

namespace CareSphere.Data.Tests
{
    [SetUpFixture]
    public abstract class TestBase
    {
        private static MsSqlContainer _dbContainer;
        protected static string ConnectionString;
        protected IServiceProvider ServiceProvider { get; private set; }
        protected IConfiguration Configuration { get; private set; }
        private static bool _databaseInitialized;
        private string _dacpacPath;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _dbContainer = new MsSqlBuilder()
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest") // Use SQL Server 2022 image
                .Build();

            await _dbContainer.StartAsync();
            ConnectionString = _dbContainer.GetConnectionString() + ";Database=TestDb";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            if (!_databaseInitialized)
            {
                await DeploySchema();
                await SeedDatabase();
                _databaseInitialized = true;
            }
            
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            //services.AddDbContext<CareSphereDbContext>(options => options.UseSqlServer(ConnectionString));
            //services.AddDbContext<OrderDbContext>(options => options.UseSqlServer(ConnectionString));
            services.AddLogging(configure => configure.AddConsole());
            //services.AddSingleton<IConfiguration>(Configuration);
            services.AddDataLayer(options => options.UseSqlServer(ConnectionString));
        }

        private async Task DeploySchema()
        {
            var dacServices = new DacServices(ConnectionString);
            _dacpacPath = GetDacpacPath();
            using (DacPackage dacpac = DacPackage.Load(_dacpacPath))
            {
                dacServices.Deploy(dacpac, "TestDb", upgradeExisting: true);
            }

            // Add a delay to ensure the database has enough time to process the schema changes
            await Task.Delay(5000);

            // Verify that the Organization table exists
            using (var scope = ServiceProvider.CreateScope())
            {
                var careSphereContext = scope.ServiceProvider.GetRequiredService<CareSphereDbContext>();
                var organizationExists = await careSphereContext.Database.ExecuteSqlRawAsync("SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Organization'");

                if (organizationExists == 0)
                {
                    throw new InvalidOperationException("The Organization table was not created.");
                }
            }
        }

        private string GetDacpacPath()
        {
            // Find the solution directory
            string solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

            // Construct the path to the .dacpac file
            string dacpacPath = Path.Combine(solutionDirectory, "CareSphere.DB", "bin", "Debug", "CareSphere.DB.dacpac");

            if (!File.Exists(dacpacPath))
            {
                throw new FileNotFoundException($"Dacpac file not found at path: {dacpacPath}");
            }

            return dacpacPath;
        }

        private async Task SeedDatabase()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var careSphereContext = scope.ServiceProvider.GetRequiredService<CareSphereDbContext>();
                var orderContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                await ClearDatabase(careSphereContext);
                await ClearDatabase(orderContext);

                await InsertTestData(careSphereContext);
                await InsertTestData(orderContext);
            }
        }

        private async Task ClearDatabase(DbContext context)
        {
            if (context is CareSphereDbContext careSphereContext)
            {
                if (await careSphereContext.Database.CanConnectAsync())
                {
                    careSphereContext.Organization.RemoveRange(careSphereContext.Organization);
                    careSphereContext.User.RemoveRange(careSphereContext.User);
                }
            }

            if (context is OrderDbContext orderContext)
            {
                if (await orderContext.Database.CanConnectAsync())
                {
                    orderContext.Orders.RemoveRange(orderContext.Orders);
                    orderContext.OrderItems.RemoveRange(orderContext.OrderItems);
                }
            }

            await context.SaveChangesAsync();
        }

        private async Task InsertTestData(DbContext context)
        {
            if (context is CareSphereDbContext careSphereContext)
            {
                using var transaction = await careSphereContext.Database.BeginTransactionAsync();
                try
                {
                    // ✅ Disable Change Tracking for Performance
                    careSphereContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    // Organization Table
                    await careSphereContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Organization] ON");
                    var organizations = LoadJsonData<Organization>("organizations.json");
                    careSphereContext.Set<Organization>().AddRange(organizations);
                    await careSphereContext.SaveChangesAsync();
                    await careSphereContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [Organization] OFF");

                    // User Table
                    await careSphereContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [User] ON");
                    var users = LoadJsonData<User>("users.json");
                    careSphereContext.Set<User>().AddRange(users);
                    await careSphereContext.SaveChangesAsync();
                    await careSphereContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [User] OFF");

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine($"Error inserting Organizations/Users: {ex.Message}");
                    throw;
                }
                finally
                {
                    careSphereContext.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }

            if (context is OrderDbContext orderContext)
            {
                using var orderTransaction = await orderContext.Database.BeginTransactionAsync();
                try
                {
                    orderContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    // ✅ Load DTOs from JSON
                    var orderDtos = LoadJsonData<OrderDto>("orders.json");

                    // ✅ Convert DTOs to `Order` objects using constructors
                    var orders = orderDtos.Select(dto => new Order(dto.Id, dto.Status)).ToList();

                    orderContext.Orders.AddRange(orders);
                    await orderContext.SaveChangesAsync();
                    await orderTransaction.CommitAsync();

                    Console.WriteLine("✅ Orders inserted successfully");

                    // ✅ Verify orders exist
                    var validOrderIds = await orderContext.Orders.Select(o => o.Id).ToListAsync();
                    if (!validOrderIds.Any())
                    {
                        throw new Exception("⚠ Error: No orders found in the database after insert!");
                    }
                }
                catch (Exception ex)
                {
                    await orderTransaction.RollbackAsync();
                    Console.WriteLine($"Error inserting Orders: {ex.Message}");
                    throw;
                }
                finally
                {
                    orderContext.ChangeTracker.AutoDetectChangesEnabled = true;
                }

                // ✅ Insert OrderItems in a separate transaction
                using var itemTransaction = await orderContext.Database.BeginTransactionAsync();
                try
                {
                    orderContext.ChangeTracker.AutoDetectChangesEnabled = false;

                    // ✅ Load DTOs from JSON
                    var orderItemDtos = LoadJsonData<OrderItemDto>("orderitems.json");

                    // ✅ Ensure only OrderItems with valid `OrderId`s are inserted
                    var validOrderIds = await orderContext.Orders.Select(o => o.Id).ToListAsync();
                    var orderItems = orderItemDtos
                        .Where(dto => validOrderIds.Contains(dto.OrderId)) // ✅ Keep only valid order items
                        .Select(dto => new OrderItem(dto.Id, dto.OrderId, dto.ProductId, dto.Quantity, dto.Price)) // ✅ Use constructor
                        .ToList();

                    if (!orderItems.Any())
                    {
                        throw new Exception("⚠ Error: No valid OrderItems found referencing existing Orders.");
                    }

                    orderContext.OrderItems.AddRange(orderItems);
                    await orderContext.SaveChangesAsync();
                    await itemTransaction.CommitAsync();

                    Console.WriteLine("✅ OrderItems inserted successfully");
                }
                catch (Exception ex)
                {
                    await itemTransaction.RollbackAsync();
                    Console.WriteLine($"Error inserting OrderItems: {ex.Message}");
                    throw;
                }
                finally
                {
                    orderContext.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }
        }




        private List<T> LoadJsonData<T>(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "SeedData", fileName);
            var jsonData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonData);
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _dbContainer.StopAsync();
        }
    }
    public class OrderDto
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
