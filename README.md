# Requirements:
- Visual Studio Code
- C# Dev Kit for Visual Studio Code
- .NET 8.0.4 SDK or .NET 8.0 SDK

# Use the EF Core Migrations feature to create the database. 
Migrations is a set of tools that create and update a database to match the data model.
Copy and run the following .NET CLI to create and update the data base

`dotnet ef migrations add InitialCreate`
`dotnet ef database update`


 The AafebenDbContext object handles the task of connecting to the database and mapping Models objects to database records. 
 The database context is registered with the Dependency Injection container in the Program.cs file:

 `
 var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AafebenDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultAafeben")));
 `


  as in the docs  [see](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/working-with-sql?view=aspnetcore-9.0&tabs=visual-studio)
