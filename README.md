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

|Service Name|Url|
|----|--|
|Auction Service|http://localhost:7001|
