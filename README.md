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
| Bidding Service  | http://localhost:7003 |
| Gateway Service  | http://localhost:6001 |
| Next.js App      | http://localhost:3000 |



## Management Urls

RabbitMQ: http://localhost:15672/ (guest/guest in firefox)

## Identity Server

Adding templates to .net core

> dotnet new --install Duende.IdentityServer.Templates

## Gateway Info

## Testing

### Testing Doubles

- Fake - Something that has a working implementation, but is not the `real` implementation.
- Mock - Something that returns a certain value when called with particular arguments.
- Stub - Some object that just returns a predefined value.

### Unit Testing naming convention

`{MethodName}_{Scenario}_{ExpectedResult}`

Example: `HasReservePrice_ReservePriceGreaterThanZero_True()`

### Next.js setup

[Next.js](https://nextjs.org/)

[Installation and setup](https://nextjs.org/docs/getting-started/installation)

Run the App

> ~/frontend/web-app/npm run dev

[View app locally](http://localhost:3000)

- Note: this requires Node version 18.17 (or higher)

[React Icons](https://react-icons.github.io/react-icons/)

[Flowbite components](https://www.flowbite-react.com/)

[Zustand docs](https://docs.pmnd.rs/zustand/getting-started/introduction)

#### Client Auth

Using [next-Auth.js](https://next-auth.js.org/providers/duende-identityserver6) for our auth library

##### .env.local

You will need to add the file .env.local in the **web-app** directory

```
NEXTAUTH_SECRET=ValueYourChoose
NEXTAUTH_URL=http://localhost:3000

```


branch: feat/ClientNotf