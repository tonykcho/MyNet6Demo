#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY MyNet6Demo.sln .
COPY ["MyNet6Demo.Api", "MyNet6Demo.Api/"]
COPY ["MyNet6Demo.Domain", "MyNet6Demo.Domain/"]

RUN dotnet restore "MyNet6Demo.Api/MyNet6Demo.Api.csproj"
WORKDIR "/src/MyNet6Demo.Api/"
RUN dotnet build "MyNet6Demo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyNet6Demo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyNet6Demo.Api.dll"]