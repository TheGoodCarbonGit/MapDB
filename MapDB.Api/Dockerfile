FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["MapDB.Api.csproj", "./"]
RUN dotnet restore "MapDB.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet publish "MapDB.Api.csproj" -c $configuration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MapDB.Api.dll"]
