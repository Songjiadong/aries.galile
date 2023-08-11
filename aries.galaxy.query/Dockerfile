#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["aries.graph.query/aries.graph.query.csproj", "aries.graph.query/"]
RUN dotnet restore "aries.graph.query/aries.graph.query.csproj"
COPY . .
WORKDIR "/src/aries.graph.query"
RUN dotnet build "aries.graph.query.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "aries.graph.query.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "aries.graph.query.dll"]