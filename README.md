# Tatami Arena

A Windows App SDK with WinUI 3 application for managing karate tournaments.

## Features

- Competition Management
  - Create and manage competitions
  - Add competition categories (Kata, Kumite)
  - Define weight and age ranges for categories
- Competitor Management
  - Add and manage competitors
  - Automatic category suggestions based on competitor data
  - Multiple category assignments per competitor

## Technical Requirements

- Windows 10 version 1809 (build 17763) or later
- .NET 6.0 or later
- Visual Studio 2022 with Windows App SDK workload
- SQL Server Express LocalDB

## Database

The application uses SQL Server Express LocalDB as an embedded database solution.

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio 2022
3. Restore NuGet packages
4. Build and run the application

## Project Structure

- `TatamiArena.Core` - Core business logic and data models
- `TatamiArena.Data` - Data access layer and database context
- `TatamiArena.UI` - WinUI 3 user interface 