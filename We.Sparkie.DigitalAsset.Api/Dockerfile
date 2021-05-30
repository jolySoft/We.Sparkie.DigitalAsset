#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["We.Sparkie.DigitalAsset.Api/We.Sparkie.DigitalAsset.Api.csproj", "We.Sparkie.DigitalAsset.Api/"]
RUN dotnet restore "We.Sparkie.DigitalAsset.Api/We.Sparkie.DigitalAsset.Api.csproj"
COPY . .
WORKDIR "/src/We.Sparkie.DigitalAsset.Api"
RUN dotnet build "We.Sparkie.DigitalAsset.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "We.Sparkie.DigitalAsset.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "We.Sparkie.DigitalAsset.Api.dll"]
