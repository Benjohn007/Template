# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the .sln file and restore dependencies
COPY BillCollector.sln ./
COPY BillCollector.API/BillCollector.API.csproj BillCollector.API/
COPY BillCollector.Application/BillCollector.Application.csproj BillCollector.Application/
COPY BillCollector.Core/BillCollector.Core.csproj BillCollector.Core/
COPY BillCollector.Infrastructure/BillCollector.Infrastructure.csproj BillCollector.Infrastructure/
COPY BillCollector.Tests/BillCollector.Tests.csproj BillCollector.Tests/

RUN dotnet restore

# Copy the remaining files and build the application
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Copy the certificate file
COPY certificate.pfx /https/certificate.pfx

# Expose the ports the application runs on
EXPOSE 5026
EXPOSE 7271

# Set environment variables
ENV ASPNETCORE_URLS="http://+:5026;https://+:7271"
ENV ASPNETCORE_ENVIRONMENT="Development"

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "BillCollector.API.dll"]

