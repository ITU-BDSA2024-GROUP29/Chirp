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

## Domaim model
The Domain model for Chirp! can be seen illustrated as an ER-diagram in the following image:
![alt text](./diagrams/drawio-assets/DomainModel-Side-1.png)
Our Cheep entity is represents all Cheeps from all users. The entity contains information about the cheeps content, when it was posted and who posted it.
The Author entity contains information such as a username and e-mail. The author entity has a relation to the Cheep entity through a list of cheeps inside the author. This is necessary to find all cheeps belonging to a specific user quickly rather than matching a specific user to all cheeps in the database.
The Chirp! application has a follow function which is also represented in the Author entity. This is made as a relation to itself, an Author can follow and be followed by many other Authors. 


## Architecture - in the small
![alt text](./diagrams/drawio-assets/Architecture-small-Side-1.png)

## Architecture of deployed application

## User activities

## Sequence of functionalities/call through Chirp

# Process

## Build, test, release and deployment

## Team work

## How to make Chirp! work locally

## How to run test suite locally

To run our test project, follow the list below:

* Have .NET 9.0 in your environment.
* Go to the project root directory (./CHIRP).
* Alternatively, go to a specific test directory (./CHIRP/test/ChirpRazor.Tests).
* Run the "dotnet test" command in your terminal.

In our test project, we cover relevant unit and integration tests for all methods within our application core. See subsection 'Architecture in the Small.'
"Relevant" tests, in this case, cover important and central methods for our Chirp application. This mainly includes methods used to interact with the database (Send Cheeps, Store Cheeps, etc).


# Ethics

## License

Chirp is made available under the [MIT License](https://opensource.org/license/mit).

[![License: MIT](https://img.shields.io/badge/License-MIT-orange.svg)](https://opensource.org/licenses/MIT)

## LLMs, ChatGPT, CoPilot and others

ChatGPT, was used during the development of this project, although it was mainly used to troubleshoot and fix errors.
