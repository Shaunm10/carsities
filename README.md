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

Runs the dockerfile to build an image
> docker build -f src/AuctionService/Dockerfile -t testing_123 .

Run the Docker image
> docker run testing_123

Build a docker image via the docker compose
> docker compose build auction-svc

## Service Urls

| Service Name     | Url                   |
| ---------------- | --------------------- |
| Auction Service  | http://localhost:7001 |
| Search Service   | http://localhost:7002 |
| Identity Service | http://localhost:5000 |
| Gateway Service | http://localhost:6001 |


## Management Urls

RabbitMQ: http://localhost:15672/ (guest/guest in firefox)

## Identity Server

Adding templates to .net core

> dotnet new --install Duende.IdentityServer.Templates

## Gateway Info
