# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution file and project files
COPY *.sln ./
COPY CareSphere.Domains/*.csproj CareSphere.Domains/
COPY CareSphere.DB/*.sqlproj CareSphere.DB/
COPY CareSphere.Data.Core/*.csproj CareSphere.Data.Core/
COPY CareSphere.Data.Orders/*.csproj CareSphere.Data.Orders/
COPY CareSphere.Data.Organizations/*.csproj CareSphere.Data.Organizations/
COPY CareSphere.Services.Configurations/*.csproj CareSphere.Services.Configurations/
COPY CareSphere.Services.Organizations/*.csproj CareSphere.Services.Organizations/
COPY CareSphere.Services.Users/*.csproj CareSphere.Services.Users/
COPY CareSphere.Services.Orders/*.csproj CareSphere.Services.Orders/
COPY CareSphere.TestUtilities/*.csproj CareSphere.TestUtilities/
COPY CareSphere.Services.Tests/*.csproj CareSphere.Services.Tests/
COPY CareSphere.Web.Server/*.csproj CareSphere.Web.Server/
COPY CareSphere.Data.Tests/*.csproj CareSphere.Data.Tests/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the application code
COPY CareSphere.TestUtilities/. CareSphere.TestUtilities/
COPY CareSphere.Services.Tests/. CareSphere.Services.Tests/
COPY CareSphere.Web.Server/. CareSphere.Web.Server/
COPY CareSphere.Data.Tests/. CareSphere.Data.Tests/
COPY CareSphere.DB/. CareSphere.DB/
COPY CareSphere.Data.Core/. CareSphere.Data.Core/
COPY CareSphere.Data.Organizations/. CareSphere.Data.Organizations/
COPY CareSphere.Domains/. CareSphere.Domains/
COPY CareSphere.Services.Configurations/. CareSphere.Services.Configurations/
COPY CareSphere.Services.Organizations/. CareSphere.Services.Organizations/
COPY CareSphere.Services.Users/. CareSphere.Services.Users/
COPY CareSphere.Data.Orders/. CareSphere.Data.Orders/
COPY CareSphere.Services.Orders/. CareSphere.Services.Orders/

# Build the application
RUN dotnet build --no-restore -c Release

# Run the tests
RUN dotnet test --no-restore --logger:trx --results-directory /app/TestResults

# Use the official .NET runtime image as the base image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /app .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "CareSphere.Web.Server.dll"]
