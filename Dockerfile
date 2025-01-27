# Use the .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy and restore dependencies
COPY *.sln ./
COPY OrderProcessingSystem/*.csproj ./OrderProcessingSystem/
COPY OrderManagement.Tests/*.csproj ./OrderManagement.Tests/
RUN dotnet restore

# Build the application
COPY . ./
RUN dotnet publish -c Release -o out

# Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .

# Expose the port
EXPOSE 5000

# Run the application
ENTRYPOINT ["dotnet", "OrderProcessingSystem.dll"]