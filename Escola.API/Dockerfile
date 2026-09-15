FROM mcr.microsoft.com/dotnet/aspnet:10.0-nanoserver-ltsc2022 AS base
WORKDIR /app
EXPOSE 5160

ENV ASPNETCORE_URLS=http://+:5160

FROM mcr.microsoft.com/dotnet/sdk:10.0-nanoserver-ltsc2022 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Escola.API/Escola.API.csproj", "Escola.API/"]
RUN dotnet restore "Escola.API\Escola.API.csproj"
COPY . .
WORKDIR "/src/Escola.API"
RUN dotnet build "Escola.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Escola.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Escola.API.dll"]
