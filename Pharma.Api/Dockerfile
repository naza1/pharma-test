#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Pharma.Api/Pharma.Api.csproj", "Pharma.Api/"]
COPY ["Pharma.Common/Pharma.Common.csproj", "Pharma.Common/"]
COPY ["Pharma.Database/Pharma.Database.csproj", "Pharma.Database/"]
RUN dotnet restore "Pharma.Api/Pharma.Api.csproj"
COPY . .
WORKDIR "/src/Pharma.Api"
RUN dotnet build "Pharma.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pharma.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pharma.Api.dll"]