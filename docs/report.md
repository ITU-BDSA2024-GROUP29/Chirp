<!-- vscode-markdown-toc -->
* 1. [Domain model](#Domainmodel)
* 2. [Architecture - in the small](#Architecture-inthesmall)
* 3. [Architecture of deployed application](#Architectureofdeployedapplication)
* 4. [User activities](#Useractivities)
* 5. [Sequence of functionalities/call through Chirp](#SequenceoffunctionalitiescallthroughChirp)
* 6. [Build, test, release and deployment](#Buildtestreleaseanddeployment)
* 7. [Team work](#Teamwork)
* 8. [Missing Features](#MissingFeatures)
	* 8.1. [DTOs](#DTOs)
	* 8.2. ["Forget me" feature](#Forgetmefeature)
	* 8.3. [End-to-end test](#End-to-endtest)
	* 8.4. [Security](#Security)
	* 8.5. [Bugs](#Bugs)
* 9. [How to Run Chirp! Locally](#HowtoRunChirpLocally)
	* 9.1. [Setting up the Chirp application](#SettinguptheChirpapplication)
	* 9.2. [Install Dependencies](#InstallDependencies)
	* 9.3. [Setting up Application User Secret](#SettingupApplicationUserSecret)
	* 9.4. [Starting the Application](#StartingtheApplication)
* 10. [How to run test suite locally](#Howtoruntestsuitelocally)
* 11. [License](#License)
* 12. [LLMs, ChatGPT, CoPilot and others](#LLMsChatGPTCoPilotandothers)

<!-- vscode-markdown-toc-config
	numbering=true
	autoSave=true
	/vscode-markdown-toc-config -->
<!-- /vscode-markdown-toc -->
---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2024 Group `29`
author:
- "Jonas Christian Henriksen <chjh@itu.dk>"
- "Adrian Hoff <adho@itu.dk>"
- "Viktor Emil Nørskov Andersen <Viea@itu.dk>"
numbersections: true
---

- [Design and Architecture](#design-and-architecture)
  - [1. Domain model](#1-domain-model)
  - [2. Architecture - in the small](#2-architecture---in-the-small)
  - [3. Architecture of deployed application](#3-architecture-of-deployed-application)
  - [4. User activities](#4-user-activities)
  - [5. Sequence of functionalities/call through Chirp](#5-sequence-of-functionalitiescall-through-chirp)
- [Process](#process)
  - [6. Build, test, release and deployment](#6-build-test-release-and-deployment)
  - [7. Team work](#7-team-work)
  - [8. Missing Features](#8-missing-features)
    - [8.1. DTOs](#81-dtos)
    - [8.2. "Forget me" feature](#82-forget-me-feature)
    - [8.3. End-to-end test](#83-end-to-end-test)
    - [8.4. Security](#84-security)
    - [8.5. Bugs](#85-bugs)
  - [9. How to Run Chirp! Locally](#9-how-to-run-chirp-locally)
    - [9.1. Setting up the Chirp application](#91-setting-up-the-chirp-application)
    - [9.2. Install Dependencies](#92-install-dependencies)
    - [9.3. Setting up Application User Secret](#93-setting-up-application-user-secret)
    - [9.4. Starting the Application](#94-starting-the-application)
  - [10. How to run test suite locally](#10-how-to-run-test-suite-locally)
- [Ethics](#ethics)
  - [11. License](#11-license)
  - [12. LLMs, ChatGPT, CoPilot and others](#12-llms-chatgpt-copilot-and-others)




https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_12/README_REPORT.md
![Test badge](https://github.com/ITU-BDSA2024-GROUP29/Chirp/actions/workflows/test.yml/badge.svg??event=push)
![Deploy badge](https://github.com/ITU-BDSA2024-GROUP29/Chirp/actions/workflows/main_bdsagroup29chirpremotedb.yml/badge.svg??event=push)


cheat sheet: https://www.markdownguide.org/cheat-sheet/

# Design and Architecture
##  1. <a name='Domainmodel'></a>Domain model
The Domain model for Chirp! can be seen illustrated as an ER-diagram in the following image:
![alt text](./diagrams/drawio-assets/DomainModel-Side-1.png)
Our Cheep entity is represents all Cheeps from all users. The entity contains information about the cheeps content, when it was posted and who posted it.
The Author entity contains information such as a username and e-mail. The author entity has a relation to the Cheep entity through a list of cheeps inside the author. This is necessary to find all cheeps belonging to a specific user quickly rather than matching a specific user to all cheeps in the database.
The Chirp! application has a follow function which is also represented in the Author entity. This is made as a relation to itself, an Author can follow and be followed by many other Authors. 

The Domain model for Chirp! can be seen illustrated as an ER-diagram in the following image:
![Chirp! ER-diagram](./diagrams/drawio-assets/DomainModel-Side-1.png)
Our Cheep entity represents all Cheeps from all users. The entity contains information about the cheeps content, when it was posted and who posted it.
The Author entity contains information such as a username and e-mail. The author entity has a relation to the Cheep entity through a list of cheeps inside the author. This is necessary to find all cheeps belonging to a specific user quickly rather than matching a specific user to all cheeps in the database.
The Chirp! application has a follow function which is also represented in the Author entity. This is made as a relation to itself, an Author can follow and be followed by many other Authors.

##  2. <a name='Architecture-inthesmall'></a>Architecture - in the small

Chirps onion architecture can be seen in the next figure below.
![Onion Architecture](./diagrams/drawio-assets/Architecture-small-Side-1.png)
The onion architecture diagram consists of four layers, the Core, Repository, Service and Razor.
They are called the same as their respective folders.

* The red "Application Core" is a term we will use to reference the parts that deal with logic and data processing.

* The Core section has our DomainModel, this includes the DBContext, Cheep and Author class. This is also where IdentityCore is implemented.

* The Repository and Service layers each contain different logic methods and responsibilities.

* The Razor layer is what is generally recognized as the web layer, the folder is just called 'Chirp.Razor'.
this layer contains the webpages, database and startup program for Chirp! tests are also illustrated in this layer, although they are in a separate folder.

##  3. <a name='Architectureofdeployedapplication'></a>Architecture of deployed application

##  4. <a name='Useractivities'></a>User activities

##  5. <a name='SequenceoffunctionalitiescallthroughChirp'></a>Sequence of functionalities/call through Chirp

# Process

##  6. <a name='Buildtestreleaseanddeployment'></a>Build, test, release and deployment

##  7. <a name='Teamwork'></a>Team work

##  8. <a name='MissingFeatures'></a>Missing Features
###  8.1. <a name='DTOs'></a>DTOs
###  8.2. <a name='Forgetmefeature'></a>"Forget me" feature
###  8.3. <a name='End-to-endtest'></a>End-to-end test
###  8.4. <a name='Security'></a>Security
###  8.5. <a name='Bugs'></a>Bugs

##  9. <a name='HowtoRunChirpLocally'></a>How to Run Chirp! Locally

To run the Chirp application locally, you will first need to set up your environment.

###  9.1. <a name='SettinguptheChirpapplication'></a>Setting up the Chirp application

- Clone the Git repository using `git clone https://github.com/ITU-BDSA2024-GROUP29/Chirp.git`
- Install dotnet 9 [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)

###  9.2. <a name='InstallDependencies'></a>Install Dependencies

To restore and install the project dependencies, navigate to the project root directory and run: `dotnet restore`

###  9.3. <a name='SettingupApplicationUserSecret'></a>Setting up Application User Secret

- Navigateto directory `cd .\src\Chirp.Razor\`
- `dotnet user-secrets init`
- `dotnet user-secrets set "authentication:github:clientSecret" "3953dca60cd3ab410fe0649ae2d02c71160eeff1" `
- `dotnet user-secrets set "authentication:github:clientId" "Ov23liqlfyf9uGVeeLpF" `

###  9.4. <a name='StartingtheApplication'></a>Starting the Application

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

##  10. <a name='Howtoruntestsuitelocally'></a>How to run test suite locally

To run our test project, follow the list below:

* Have [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
 in your environment.
* Go to the project root directory `./CHIRP`
* Alternatively, go to a specific test directory `/CHIRP/test/ChirpRazor.Tests`
* Run the `dotnet test` command in your terminal.

In our test project, we cover relevant unit and integration tests for all methods within our application core. See subsection 'Architecture in the Small'   
Relevant tests, in this case, cover important and central methods for our Chirp application. This mainly includes methods used to interact with the database (Send Cheeps, Store Cheeps, etc).


# Ethics

##  11. <a name='License'></a>License

Chirp is available under the [MIT License](https://opensource.org/license/mit).  
The application dependencies are also licensed under the [MIT License](https://opensource.org/license/mit).

[![License: MIT](https://img.shields.io/badge/License-MIT-orange.svg)](https://opensource.org/licenses/MIT)

##  12. <a name='LLMsChatGPTCoPilotandothers'></a>LLMs, ChatGPT, CoPilot and others

ChatGPT, was used during the development of this project, although it was mainly used to troubleshoot and fix errors.
