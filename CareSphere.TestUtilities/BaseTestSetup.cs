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
using Moq;
using Moq.EntityFrameworkCore;
using NUnit.Framework;


namespace CareSphere.TestUtilities
{
    [SetUpFixture]
    public abstract class BaseTestSetup
    {
        protected static string ConnectionString;
        protected IServiceProvider ServiceProvider { get; private set; }
        protected IConfiguration Configuration { get; private set; }

        private string _dbMode;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("testsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _dbMode = Configuration.GetValue<string>("DbMode");

            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            if (_dbMode == "InMemory")
            {
                await SeedDatabase();
            }
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            switch (_dbMode)
            {
                case "InMemory":
                    Console.WriteLine("⚡ Using In-Memory Database with Seeded Data");
                    ConnectionString = "InMemoryDb";
                    ConfigureServicesForInMemory(services);
                    break;

                case "Existing":
                    Console.WriteLine("🛢️ Using Existing SQL Server (No Seeding)");
                    ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                    ConfigureServicesForSqlServer(services);
                    break;

                case "Mock":
                    Console.WriteLine("🔹 Using Mocked `DbContext` with Test Data");
                    ConfigureServicesForMock(services);
                    break;

                default:
                    throw new ArgumentException($"Invalid database mode: {_dbMode}");
            }
        }

        protected void ConfigureServicesForInMemory(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddLogging(configure => configure.AddConsole());

        
            services.AddDataLayer(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });
        }

        protected void ConfigureServicesForSqlServer(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddLogging(configure => configure.AddConsole());

       
            services.AddDataLayer(options =>
            {
                options.UseSqlServer(ConnectionString);
            });
        }

        protected void ConfigureServicesForMock(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddLogging(configure => configure.AddConsole());

            // ✅ Create Mocked `DbContext`
            var mockCareSphereDbContext = new Mock<CareSphereDbContext>(new DbContextOptions<CareSphereDbContext>());
            var mockOrderDbContext = new Mock<OrderDbContext>(new DbContextOptions<OrderDbContext>());

            // ✅ Load Seed Data
            var organizations = LoadJsonData<Organization>("organizations.json");
            var orders = LoadJsonData<OrderDto>("orders.json")
                         .Select(dto => new Order(dto.Id, dto.Status))
                         .ToList();

            // ✅ Fix: Use `ReturnsDbSet()` from the helper method
            mockCareSphereDbContext.Setup(db => db.Organization).ReturnsDbSet(organizations);
            mockOrderDbContext.Setup(db => db.Orders).ReturnsDbSet(orders);

            // ✅ Register mocks in DI container BEFORE calling AddDataLayer
            services.AddSingleton(mockCareSphereDbContext.Object);
            services.AddSingleton(mockOrderDbContext.Object);

            // ✅ Call `AddDataLayer`, ensuring it does NOT override the mocks
            services.AddDataLayer(_ => { });

            Console.WriteLine("✅ Mocked `DbContext` registered successfully");
        }








        private async Task SeedDatabase()
        {
            using var scope = ServiceProvider.CreateScope();
            var careSphereContext = scope.ServiceProvider.GetRequiredService<CareSphereDbContext>();
            var orderContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

            Console.WriteLine("🔹 Clearing existing in-memory data...");
            await ClearDatabase(careSphereContext);
            await ClearDatabase(orderContext);

            Console.WriteLine("✅ Seeding in-memory database...");
            await InsertTestData(careSphereContext);
            await InsertTestData(orderContext);
        }

        private async Task ClearDatabase(DbContext context)
        {
            if (context is CareSphereDbContext careSphereContext)
            {
                careSphereContext.Organization.RemoveRange(careSphereContext.Organization);
                careSphereContext.User.RemoveRange(careSphereContext.User);
            }
            if (context is OrderDbContext orderContext)
            {
                orderContext.Orders.RemoveRange(orderContext.Orders);
                orderContext.OrderItems.RemoveRange(orderContext.OrderItems);
            }
            await context.SaveChangesAsync();
        }

        private async Task InsertTestData(DbContext context)
        {
            bool isInMemory = context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";

            if (context is CareSphereDbContext careSphereContext)
            {
                var organizations = LoadJsonData<Organization>("organizations.json");
                careSphereContext.Organization.AddRange(organizations);

                var users = LoadJsonData<User>("users.json");
                careSphereContext.User.AddRange(users);

                await careSphereContext.SaveChangesAsync();
            }

            if (context is OrderDbContext orderContext)
            {
                if (!isInMemory) // ✅ Skip transactions for In-Memory DB
                {
                    using var transaction = await orderContext.Database.BeginTransactionAsync();
                    try
                    {
                        orderContext.ChangeTracker.AutoDetectChangesEnabled = false;

                        var orderDtos = LoadJsonData<OrderDto>("orders.json");
                        var orders = orderDtos.Select(dto => new Order(dto.Id, dto.Status)).ToList();
                        orderContext.Orders.AddRange(orders);
                        await orderContext.SaveChangesAsync();

                        var orderItemDtos = LoadJsonData<OrderItemDto>("orderitems.json");
                        var validOrderIds = orders.Select(o => o.Id).ToHashSet();
                        var orderItems = orderItemDtos
                            .Where(dto => validOrderIds.Contains(dto.OrderId))
                            .Select(dto => new OrderItem(dto.Id, dto.OrderId, dto.ProductId, dto.Quantity, dto.Price))
                            .ToList();

                        orderContext.OrderItems.AddRange(orderItems);
                        await orderContext.SaveChangesAsync();
                        await transaction.CommitAsync();

                        Console.WriteLine("✅ Orders & OrderItems seeded successfully");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.WriteLine($"❌ Error seeding Orders/OrderItems: {ex.Message}");
                        throw;
                    }
                    finally
                    {
                        orderContext.ChangeTracker.AutoDetectChangesEnabled = true;
                    }
                }
                else
                {
                    // Directly save without transaction for In-Memory DB
                    var orderDtos = LoadJsonData<OrderDto>("orders.json");
                    var orders = orderDtos.Select(dto => new Order(dto.Id, dto.Status)).ToList();
                    orderContext.Orders.AddRange(orders);
                    await orderContext.SaveChangesAsync();

                    var orderItemDtos = LoadJsonData<OrderItemDto>("orderitems.json");
                    var validOrderIds = orders.Select(o => o.Id).ToHashSet();
                    var orderItems = orderItemDtos
                        .Where(dto => validOrderIds.Contains(dto.OrderId))
                        .Select(dto => new OrderItem(dto.Id, dto.OrderId, dto.ProductId, dto.Quantity, dto.Price))
                        .ToList();

                    orderContext.OrderItems.AddRange(orderItems);
                    await orderContext.SaveChangesAsync();

                    Console.WriteLine("✅ Orders & OrderItems seeded successfully (No Transactions - InMemory)");
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
            // No database cleanup required
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
