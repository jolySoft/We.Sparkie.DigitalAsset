FROM mcr.microsoft.com/dotnet/core/runtime:2.2.7-alpine3.9 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2.402-alpine3.9 AS build
WORKDIR /src
COPY ["We.CrudService.Api.csproj", "."]
RUN dotnet restore "We.CrudService.Api.csproj"
COPY . .
RUN dotnet build "We.CrudService.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "We.CrudService.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENTRYPOINT ["dotnet", "We.CrudService.Api.dll"]