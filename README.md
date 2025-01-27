Order Processing System
Overview
The Order Processing System is an e-commerce application built with .NET Core Web API and SQL Server. This system allows managing customers, products, and orders. It includes features such as order creation, total price calculation, and customer validation to ensure no unfulfilled orders are pending before a new order can be placed.

This project demonstrates best practices in modular code, separation of concerns, and asynchronous programming.

Features
Customer Management: Create, retrieve, and manage customers.
Product Management: Add, retrieve, and manage products with prices.
Order Management: Place orders, calculate total price, and handle unfulfilled orders.
Validation: Orders cannot be placed if a customer has an unfulfilled order.
Error Handling: Includes logging and meaningful error messages.
Logging: Integrated logging using Serilog.
Database: Uses Entity Framework Core with SQL Server.
API Endpoints
1. GET /api/customers
Retrieve all customers.

Response:

Status: 200 OK
Content: List of customers with their details.
2. GET /api/customers/{id}
Retrieve details for a specific customer, including their orders.

Response:

Status: 200 OK
Content: Customer details and associated orders.
3. POST /api/orders
Create a new order for a customer by providing the customer ID and a list of product IDs.

Request:

Body:
json
Copy
Edit
{
  "customerId": 1,
  "productIds": [1, 2, 3]
}
Response:

Status: 201 Created
Content: Created order details including total price.
4. GET /api/orders/{id}
Retrieve details for a specific order, including the total price.

Response:

Status: 200 OK
Content: Order details, including total price.
Technologies Used
.NET 5/6 Web API
SQL Server (for the database)
Entity Framework Core (ORM)
Serilog (for logging)
Swagger (for API documentation)
Setup Instructions
Clone the repository:

bash
Copy
Edit
git clone https://github.com/Bijay-1996/OrderProcessingSystem.git
cd OrderProcessingSystem
Configure the Database:

Open appsettings.json and configure your SQL Server connection string.
json
Copy
Edit
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrderProcessingSystemDb;Trusted_Connection=True"
}
Run Migrations: To create the initial database and tables, run the following commands:

bash
Copy
Edit
dotnet ef migrations add InitialCreate
dotnet ef database update
Run the Application:

bash
Copy
Edit
dotnet run --project OrderProcessingSystem.Api
The application will be running at http://localhost:5000.

Error Handling and Logging
The application uses Serilog to log important actions, warnings, and errors. Logs are written to the console by default. You can configure additional log sinks like files or external services if needed.

Bonus Features
1. Unit Tests
Unit tests can be added for the business logic and validation using frameworks like xUnit or NUnit. Unit tests can ensure the correctness of methods like order price calculation and validation checks.

2. Dockerize the Application
To deploy the application with Docker, create a Dockerfile in the root of the project.

Example Dockerfile:

Dockerfile
Copy
Edit
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OrderProcessingSystem.Api/OrderProcessingSystem.Api.csproj", "OrderProcessingSystem.Api/"]
RUN dotnet restore "OrderProcessingSystem.Api/OrderProcessingSystem.Api.csproj"
COPY . .
WORKDIR "/src/OrderProcessingSystem.Api"
RUN dotnet build "OrderProcessingSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderProcessingSystem.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderProcessingSystem.Api.dll"]
Build and run the Docker container:

bash
Copy
Edit
docker build -t order-process-system .
docker run -d -p 5000:80 order-process-system
Contributing
If you'd like to contribute to this project, please fork the repository and create a pull request with your proposed changes. Ensure that your code follows the project’s coding standards and that any new functionality is covered by tests.
