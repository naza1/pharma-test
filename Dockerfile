FROM mcr.microsoft.com/dotnet/core/sdk:3.1.302-buster AS restore
WORKDIR /src
COPY Pharma.Api.sln ./
COPY ["Pharma.Api/Pharma.Api.csproj", "Pharma.Api/"]
COPY ["Pharma.Common/Pharma.Common.csproj", "Pharma.Common/"]
COPY ["Pharma.Database/Pharma.Database.csproj", "Pharma.Database/"]
RUN dotnet restore

FROM restore AS build
COPY . .
RUN dotnet build -c Release

FROM build AS test
RUN dotnet test

FROM build AS publish
RUN dotnet publish "Pharma.Api/Pharma.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.6-buster-slim AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pharma.Api.dll"]
