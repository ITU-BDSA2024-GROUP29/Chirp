---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2024 Group `29`
author:
  - "Jonas Christian Henriksen <chjh@itu.dk>"
  - "Adrian Hoff <adho@itu.dk>"
numbersections: true
---


https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_12/README_REPORT.md
![Test badge](https://github.com/ITU-BDSA2024-GROUP29/Chirp/actions/workflows/test.yml/badge.svg??event=push)
![Deploy badge](https://github.com/ITU-BDSA2024-GROUP29/Chirp/actions/workflows/main_bdsagroup29chirpremotedb.yml/badge.svg??event=push)


cheat sheet: https://www.markdownguide.org/cheat-sheet/

# Design and Architecture

## Domain model

The Domain model for Chirp! can be seen illustrated as an ER-diagram in the following image:
![Chirp! ER-diagram](./diagrams/drawio-assets/DomainModel-Side-1.png)
Our Cheep entity represents all Cheeps from all users. The entity contains information about the cheeps content, when it was posted and who posted it.
The Author entity contains information such as a username and e-mail. The author entity has a relation to the Cheep entity through a list of cheeps inside the author. This is necessary to find all cheeps belonging to a specific user quickly rather than matching a specific user to all cheeps in the database.
The Chirp! application has a follow function which is also represented in the Author entity. This is made as a relation to itself, an Author can follow and be followed by many other Authors.

## Architecture - in the small

Chirps onion architecture can be seen in the next figure below.
![Onion Architecture](./diagrams/drawio-assets/Architecture-small-Side-1.png)
The onion architecture diagram consists of four layers, the Core, Repository, Service and Razor.
They are called the same as their respective folders.

The red "Application Core" is a term we will use to reference the parts that deal with logic and data processing.

The Core section has our DomainModel, this includes the DBContext, Cheep and Author class. This is also where IdentityCore is implemented.

The Repository and Service layers each contain different logic methods and responsibilities.

The Razor layer is what is generally recognized as the web layer, the folder is just called 'Chirp.Razor'.
this layer contains the webpages, database and startup program for Chirp! tests are also illustrated in this layer, although they are in a separate folder.

## Architecture of deployed application

## User activities
![User Activities](./diagrams/drawio-assets/UserJourney-small-Side-1.png)

## Sequence of functionalities/call through Chirp

# Process

## Build, test, release and deployment

## Team work

## Missing Features
### DTOs
### "Forget me" feature
### End-to-end test
### Security
### Bugs

## How to Run Chirp! Locally

To run the Chirp application locally, you will first need to set up your environment.

### Setting up the Chirp application

- Clone the Git repository using `git clone https://github.com/ITU-BDSA2024-GROUP29/Chirp.git`
- Install dotnet 9 [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)

### Install Dependencies

To restore and install the project dependencies, navigate to the project root directory and run: `dotnet restore`

### Setting up Application User Secret

- Navigateto directory `cd .\src\Chirp.Razor\`
- `dotnet user-secrets init`
- `dotnet user-secrets set "authentication:github:clientSecret" "3953dca60cd3ab410fe0649ae2d02c71160eeff1" `
- `dotnet user-secrets set "authentication:github:clientId" "Ov23liqlfyf9uGVeeLpF" `

### Starting the Application

- Navigate to the Chirp.Razor directory: `cd .\src\Chirp.Razor\ `
- Run the application with the following command: `dotnet run`
- You will see the local IP displayed in the terminal, similar to: `http://localhost:5273`

> [!NOTE]
> The application relies on several dependencies defined in the .csproj file. These include:

`Microsoft.EntityFrameworkCore.Sqlite` for database operations
`AspNet.Security.OAuth.GitHub` for GitHub authentication
`Microsoft.AspNetCore.Identity` for identity management

To ensure all dependencies are installed, use: `dotnet restore`

For database migrations and other Entity Framework tasks, ensure you have the EF CLI installed:
`dotnet tool install --global dotnet-ef`

## How to run test suite locally

# Ethics

## License

Chirp is available under the [MIT License](https://opensource.org/license/mit).  
The application dependencies are also licensed under the [MIT License](https://opensource.org/license/mit).

[![License: MIT](https://img.shields.io/badge/License-MIT-orange.svg)](https://opensource.org/licenses/MIT)

## LLMs, ChatGPT, CoPilot and others

ChatGPT, was used during the development of this project, although it was mainly used to troubleshoot and fix errors.
