FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["01_UserInterface（前端界面）/FOPS.Agent/FOPS.Agent.csproj", "FOPS.Agent/"]
RUN dotnet restore "01_UserInterface（前端界面）/FOPS.Agent/FOPS.Agent.csproj"
COPY . .
WORKDIR "/src/FOPS.Agent"
RUN dotnet build "FOPS.Agent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FOPS.Agent.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FOPS.Agent.dll"]
