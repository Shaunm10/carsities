# Carsities

## Entity Framework commands

Install the Ef Tool (inside the project)

> dotnet tool install dotnet ef -g

Add's a EF Migration

> dotnet ef migrations add 'Initial Creation' -o Data/Migrations

Update the database by running all migrations

> dotnet ef database update

## Docker commands

Run docker compose file in detached mode

> docker compose up -d

## Service Urls

| Service Name    | Url                   |
| --------------- | --------------------- |
| Auction Service | http://localhost:7001 |
| Search Service  | http://localhost:7002 |
| Identity Service| http://localhost:5000 |


## Management Urls

RabbitMQ: http://localhost:15672/ (guest/guest in firefox)


## Identity Server

Adding templates to .net core
> dotnet new --install Duende.IdentityServer.Templates