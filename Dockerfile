# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln .
COPY CareSphere.TestUtilities/*.csproj CareSphere.TestUtilities/
COPY CareSphere.Services.Tests/*.csproj CareSphere.Services.Tests/
COPY CareSphere.Web.Server/*.csproj CareSphere.Web.Server/
RUN dotnet restore

# Copy the rest of the application code
COPY CareSphere.TestUtilities/. CareSphere.TestUtilities/
COPY CareSphere.Services.Tests/. CareSphere.Services.Tests/
COPY CareSphere.Web.Server/. CareSphere.Web.Server/

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
