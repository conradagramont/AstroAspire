# AstroAspire (Basic)

This sample demonstrates an approach for integrating a Node.js, we're using [Astro](https://astro.build/), into a [.NET Aspire application](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview).

- Additional details: 
  - [Using Node/Express (like Astro) with .NET Aspire (AstroAspire Basic)](https://agramont.net/blog/astroaspire-using-node-express-astro-with-dotnet-aspire)
  - [Introducing AstroAspire: Using .NET Aspire with Astro SSR (Nodejs/Express)](https://agramont.net/blog/astroaspire-overview)

The app consists of two services:

- **AstroFrontend (Astro)**: This is a simple Astro app using Express-based Node.js app. It renders static pages, server side rendered pages, and an API (which calls AstroAspire.API)
- **AstroAspire.API (AspNetCoreApi)**: This is an HTTP API that returns randomly generated weather forecast data.

You can find the currently available posts in the series here: 

[https://agramont.net/series/astroaspire](https://agramont.net/series/astroaspire)

## Pre-requisites

- .NET 8 SDK
- [Node.js](Node.js) - at least version 20.9.0
- Optional Visual Studio Code, Visual Studio 2022 17.9 Preview or any text editor
- **Required for local deployment to Azure:** You don't need these for now just to run this project locally
  - [Azure Developer CLI](https://learn.microsoft.com/en-us/azure/developer/azure-developer-cli/install-azd)
  - [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Running the app

If using Visual Studio, open the solution file `AstroAspire.sln` and launch/debug the `AstroAspire.AppHost` project.

NOTE: The frontend folder/code will not be visible in Visual Studio.  However, the app will run and you can see the output in the browser.

If using the .NET CLI, run `dotnet run` from the `\Basic\AstroAspire.AppHost` directory.

### Project structure

Inside of the AstroAspire project (We're using the Basic folder with the GitHub repo for this series of posts), you'll see the following folders 
and files (we'll only show the relevant files for this project):

```
/
├── AstroAspire.API/
│   ├── Controllers/
│   │   └── WeatherForecastController.cs
│   ├── appsettings.json
│   └── Program.cs
├── AstroAspire.AppHost/
│   ├── appsettings.json
│   └── Program.cs
├── AstroAspire.ServiceDefaults/
│   └── Extensions.cs
├── AstroFrontend/
│   ├── src/
│   │   ├── components/
│   │   │   └── AspireRestWeather.js
│   │   ├── layouts/
│   │   │   └── Layout.astro
│   │   └── pages/
│   │       ├── api/
|   |       |   ├── get-astroaspireapi.js
│   │       │   └── get-weather.js
│   │       ├── weatherapidirect.astro
│   │       ├── weatherastroapi.astro
│   │       ├── weatherssr.astro
│   │       └── weatherstatic.astro
│   ├── app.js
│   ├── astro.config.mjs
│   └── package.json
└── AstroAspire.sln

```