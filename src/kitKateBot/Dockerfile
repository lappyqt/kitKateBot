FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["kitKateBot.csproj", "."]
RUN dotnet restore "./kitKateBot.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "kitKateBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "kitKateBot.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "kitKateBot.dll"]