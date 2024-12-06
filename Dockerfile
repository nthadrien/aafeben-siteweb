# Use the official .NET 8 SDK image as the base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory in the container
WORKDIR /app

# Copy the csproj and restore the dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the project files
COPY . .

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Create a runtime image based on the .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory in the container
WORKDIR /app

# Copy the published application
COPY --from=build-env /app/publish .

# Expose the port the app will listen on
EXPOSE 8080

# Set the command to run the application
ENTRYPOINT ["dotnet", "YourProjectName.dll"]