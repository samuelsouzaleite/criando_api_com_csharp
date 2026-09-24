FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5160

ENV ASPNETCORE_URLS=http://+:5160

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Escola.API/Escola.API.csproj", "Escola.API/"]
COPY ["Escola.Application/Escola.Application.csproj", "Escola.Application/"]
COPY ["Escola.Domain/Escola.Domain.csproj", "Escola.Domain/"]
COPY ["Escola.Infra.Data/Escola.Infra.Data.csproj", "Escola.Infra.Data/"]
COPY ["Escola.Infra.Ioc/Escola.Infra.Ioc.csproj", "Escola.Infra.Ioc/"]
RUN dotnet restore "Escola.API/Escola.API.csproj"
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