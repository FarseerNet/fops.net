FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["01_UserInterface（前端界面）/FOPS.Blazor/FOPS.Blazor.csproj", "FOPS.Blazor/"]
RUN dotnet restore "01_UserInterface（前端界面）/FOPS.Blazor/FOPS.Blazor.csproj"
COPY . .
WORKDIR "/src/FOPS.Blazor"
RUN dotnet build "FOPS.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FOPS.Blazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FOPS.Blazor.dll"]
