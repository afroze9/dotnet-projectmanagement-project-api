FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/ProjectManagement.Project.Api/ProjectManagement.Project.Api.csproj", "./"]
COPY ["src/ProjectManagement.Project.Api/nuget.config", "./"]
RUN dotnet restore --configfile nuget.config "ProjectManagement.Project.Api.csproj"

COPY ["src/ProjectManagement.Project.Api/*", "./"]
RUN dotnet build -c Release -o /app/build --no-restore "ProjectManagement.Project.Api.csproj"

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false --no-restore "ProjectManagement.Project.Api.csproj"

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectManagement.Project.Api.dll"]
