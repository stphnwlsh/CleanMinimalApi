# Clean Minimal API

## How to Run Project

There are multiple ways to run this project using two pre-configured environments `local` and `docker`.  The `appsettings.json` files are configured to run the projects in different ways and connect different application logging mechanisms.  When running locally the application logs to the console window, when running in docker it spins up [Seq](https://datalust.co/seq) in a separate container and logs there instead.

### DOTNET CLI

The CLI makes everything easier in .NET engineering.  Running this application is no different.  This task is aided by the launchsettings.json file that sets all the parameters required to run the API on your local machine.  Ports, environments and logging are all configured in the settings, so the only command required is the following single line statement.  

``` bash
dotnet run --project ./src/Presentation
```

### DOCKER COMPOSE

Docker compose makes it easier to start multiple containers at one time and manage their configuration from one file (`docker-compose.yml`).  This project will run the application and a [Seq](https://datalust.co/seq) containter for application logging.  

- App - http://localhost:8080/swagger
- Seq - http://localhost:5341

To start the Containers

``` bash
docker-compose up -d
```

To shut down the containers

``` bash
docker-compose down
```
