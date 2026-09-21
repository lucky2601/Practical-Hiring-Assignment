# PracticalHiring — Employee Management System

## Overview
An ASP.NET Core MVC application for managing Employees and Attendance records, built with Entity Framework Core (Code First), SQL Server, Bootstrap, and jQuery/AJAX.

## Tech Stack
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core (Code First + Migrations)
- SQL Server (LocalDB)
- Bootstrap 5
- jQuery / AJAX

## Prerequisites
- Visual Studio 2022 (or later)
- .NET 8 SDK
- SQL Server LocalDB (comes with Visual Studio's default workload)

## Setup Instructions
1. Clone this repository: `git clone <your-repo-url>`
2. Open `PracticalHiring.sln` in Visual Studio
3. Restore NuGet packages (Visual Studio does this automatically on open, or right-click the solution → Restore NuGet Packages)
4. Check the connection string in `appsettings.json` under `ConnectionStrings:DefaultConnection` — update it if your SQL Server instance name differs from `(localdb)\mssqllocaldb`
5. Open the Package Manager Console (Tools → NuGet Package Manager → Package Manager Console) and run: