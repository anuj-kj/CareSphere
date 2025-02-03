# ---- Build Stage ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln ./

# Copy project files before restoring (to leverage Docker caching)
COPY CareSphere.Web.Server/*.csproj CareSphere.Web.Server/
COPY CareSphere.Services.Tests/*.csproj CareSphere.Services.Tests/
COPY CareSphere.Data.Tests/*.csproj CareSphere.Data.Tests/
COPY CareSphere.TestUtilities/*.csproj CareSphere.TestUtilities/
COPY CareSphere.Data.Core/*.csproj CareSphere.Data.Core/
COPY CareSphere.Data.Organizations/*.csproj CareSphere.Data.Organizations/
COPY CareSphere.Domains/*.csproj CareSphere.Domains/
COPY CareSphere.Services.Configurations/*.csproj CareSphere.Services.Configurations/
COPY CareSphere.Services.Organizations/*.csproj CareSphere.Services.Organizations/
COPY CareSphere.Services.Users/*.csproj CareSphere.Services.Users/
COPY CareSphere.Data.Orders/*.csproj CareSphere.Data.Orders/
COPY CareSphere.Services.Orders/*.csproj CareSphere.Services.Orders/
COPY CareSphere.DB/*.sqlproj CareSphere.DB/

# Restore dependencies
RUN dotnet restore

# Copy the entire source code
COPY . ./

# Debugging: Check if all necessary files exist
RUN ls -la /app && ls -la /app/CareSphere.Data.Organizations || exit 1

# Build the application
RUN dotnet build --no-restore -c Release

# Run tests
RUN dotnet test --no-restore --logger:trx --results-directory /app/TestResults

# ---- Runtime Stage ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /app .

# Expose port
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "CareSphere.Web.Server.dll"]
