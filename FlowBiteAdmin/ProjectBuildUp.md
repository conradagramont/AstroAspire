# Create our base folder for our project

md FlowBiteAdmin
cd .\FlowBiteAdmin\

# Create the bare bones of .NET Aspire. It will derive the solution name from the folder we're going run this in.
#	Docs: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new

```powershell
dotnet new aspire
```

# Create .NET Core Controller Based API

```powershell
dotnet new webapi --use-controllers -o FlowBiteAdmin.API
```

# Create .NET Razor for Admin UI

```powershell
dotnet new razor -f net8.0 -o FlowBiteAdmin.RazorUI
```

# Create .NET Razor for Admin UI

```powershell
dotnet new webapp -f net8.0 -o FlowBiteAdmin.DBManager
```

## Create Class Library for Shared Objects

```powershell
dotnet new classlib -f net8.0 -o FlowBiteAdmin.Shared
```

# Add project refrences to Aspire AppHost

From our current solution root: Directory: C:\dev\FlowBiteAdmin
```powershell
# Add Project reference from AppHost to .NET projects 
dotnet add .\FlowBiteAdmin.AppHost\FlowBiteAdmin.AppHost.csproj reference .\FlowBiteAdmin.API\FlowBiteAdmin.API.csproj
dotnet add .\FlowBiteAdmin.AppHost\FlowBiteAdmin.AppHost.csproj reference .\FlowBiteAdmin.RazorUI\FlowBiteAdmin.RazorUI.csproj
dotnet add .\FlowBiteAdmin.AppHost\FlowBiteAdmin.AppHost.csproj reference .\FlowBiteAdmin.DBManager\FlowBiteAdmin.DBManager.csproj

# Add Project reference from .NET projects to ServiceDefaults
dotnet add .\FlowBiteAdmin.API\FlowBiteAdmin.API.csproj reference .\FlowBiteAdmin.ServiceDefaults\FlowBiteAdmin.ServiceDefaults.csproj
dotnet add .\FlowBiteAdmin.DBManager\FlowBiteAdmin.DBManager.csproj reference .\FlowBiteAdmin.ServiceDefaults\FlowBiteAdmin.ServiceDefaults.csproj
dotnet add .\FlowBiteAdmin.RazorUI\FlowBiteAdmin.RazorUI.csproj reference .\FlowBiteAdmin.ServiceDefaults\FlowBiteAdmin.ServiceDefaults.csproj

## Add project reference from to Shared Class library for those that need it
dotnet add .\FlowBiteAdmin.API\FlowBiteAdmin.API.csproj reference .\FlowBiteAdmin.Shared\FlowBiteAdmin.Shared.csproj
dotnet add .\FlowBiteAdmin.DBManager\FlowBiteAdmin.DBManager.csproj reference .\FlowBiteAdmin.Shared\FlowBiteAdmin.Shared.csproj

# Add .NET resources to the Visual Studio solution file. Keeps things friendly if you edit with Visual Studio 2022
dotnet sln .\FlowBiteAdmin.sln add .\FlowBiteAdmin.API\FlowBiteAdmin.API.csproj
dotnet sln .\FlowBiteAdmin.sln add .\FlowBiteAdmin.Shared\FlowBiteAdmin.Shared.csproj
dotnet sln .\FlowBiteAdmin.sln add .\FlowBiteAdmin.RazorUI\FlowBiteAdmin.RazorUI.csproj
```
# Create Astro project from Template

```powershell
npm create astro@latest -- --template themesberg/flowbite-astro-admin-dashboard
```

Answers to the create astro command
	- ./FlowBiteAdmin.Frontend
	- Do you plan to write TypeScript? Yes
	- How strict should TypeScript be? Strict (recommended)
	- Install dependencies? Yes
	- Initialize a new git repository? (optional) No
	
# Let's add in all the components we plan to use into the Astro project.

cd ./FlowBiteAdmin.Frontend

# NOTE: The template was behind a few version from Astro. So had to do an update.

```powershell
npx @astrojs/upgrade
```
Answers
	- Some packages have breaking changes. Continue? Yes

# Add in the Node adapter and Express

```powershell
npx astro add node
```
	- Continue? Yes

Install Express

```powershell
npm install express
```

# Add in Nodemon to use for local development

```powershell
npm install --save-dev nodemon
```

# Add in components for telemetry and health checks.

```powershell
npm install @opentelemetry/sdk-node @opentelemetry/api @opentelemetry/auto-instrumentations-node @opentelemetry/sdk-metrics @opentelemetry/sdk-trace-node
npm install @opentelemetry/sdk-logs @opentelemetry/exporter-logs-otlp-grpc @opentelemetry/exporter-metrics-otlp-grpc @opentelemetry/exporter-trace-otlp-grpc @godaddy/terminus
```

# Add NodeJS to AppHost

```powershell
# Taking the long way. Go down to the root of our project
cd ..

# Go to the AppHost folder.
cd .\FlowBiteAdmin.AppHost\

# Now let's add the NodeJS provider we plan to use.
dotnet add package Aspire.Hosting.NodeJs

# We now have as much as we can setup via the command line.

dotnet dev-certs https --trust
```

# Add to Aspire.MongoDB.Driver (API and DBManager)

Since we'll use this within the .NET API, we'll need to run the following command in that folder.

Documentation: https://learn.microsoft.com/en-us/dotnet/aspire/database/mongodb-component?tabs=dotnet-cli 

Run the following command in each of these folders
- Folder: \FlowBiteAdmin\FlowBiteAdmin.API
- Folder: \FlowBiteAdmin\FlowBiteAdmin.DBManager

```powershell
dotnet add package Aspire.MongoDB.Driver
```
# Add to AppHost package Aspire.Hosting.MongoDB

```powershell
dotnet add package Aspire.Hosting.MongoDB
```powershell


