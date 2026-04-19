# Gym Portal (CoreFitness) – ASP.NET Core MVC

A full-stack gym web portal built with ASP.NET Core MVC following Domain-Driven Design (DDD) and Clean Architecture principles. The application allows users to register account, modify profile and browse information on this example of a gym portal.

## Features

### User Account

* User registration
* Login and logout
* ASP.NET Core Identity authentication
* Edit personal information
* Delete account and associated data

### Membership

* View membership prices

### My Account

* View account information
* Create, modify and delete profile

## Architecture

The solution follows:

* Domain-Driven Design (DDD)
* Clean Architecture
* Single Responsibility Principle
* Dependency Inversion Principle
* Separation of concerns across layers:

  * Presentation (MVC)
  * Application
  * Domain
  * Infrastructure

Design patterns used:

* Repository Pattern
* Result Pattern
* Base Entity Pattern
* Service Layer Pattern

## Database

* Entity Framework Core (Code First)
* SQL relational database
* Migrations supported
* InMemory database used for development/testing

## Security

* ASP.NET Core Identity authentication
* Authorization attributes for protected routes
* Server-side validation using ModelState
* Client-side validation enabled

## Testing

* Unit tests for domain logic
* Integration tests using InMemory database
* Tests located in separate test project

## Technologies Used

* ASP.NET Core MVC
* Entity Framework Core
* ASP.NET Core Identity
* SQL Server
* InMemory Database
* xUnit

## Getting Started

### Prerequisites

* .NET 8 SDK (or your project version)
* SQL Server / LocalDB
* Visual Studio or VS Code

### Run Locally

1. Clone repository

```
git clone https://github.com/Barbelito/aspnet-sharbel-kaselias.git
```

2. Navigate to project

```
cd aspnet-sharbel-kaselias
```

3. Apply migrations

```
dotnet ef database update
```

4. Run application

```
dotnet run
```

5. Open browser

```
https://0.0.0.0:4443 (for https-dev)
https://0.0.0.0:443 (for https-prod)
```

## Project Structure

```
src/
 ├── Presentation.WebApp
 ├── Application
 ├── Domain
 ├── Infrastructure
 └── Tests
```

## Notes

This project was developed as part of an ASP.NET Core assignment.
The focus was on architecture, authentication, data modeling, and testing.

## Author

Sharbel Kas Elias
