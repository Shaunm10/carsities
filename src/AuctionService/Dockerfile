FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /src
EXPOSE 80

# copy all .csproj files and restore packages. Use of the same COPY command
# for every dockerfile in the project to take advantage of docker cacheing
COPY Carsities.sln Carsities.sln
COPY src/AuctionService/AuctionService.csproj src/AuctionService/AuctionService.csproj
COPY src/SearchService/SearchService.csproj src/SearchService/SearchService.csproj
COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj



