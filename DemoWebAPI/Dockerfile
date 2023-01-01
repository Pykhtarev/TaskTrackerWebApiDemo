#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DemoWebApi/DemoWebApi.csproj", "DemoWebApi/"]
COPY ["DemoWebApi_BAL/DemoWebApi_BAL.csproj", "DemoWebApi_BAL/"]
COPY ["DemoWebApi_DA/DemoWebApi_DAL.csproj", "DemoWebApi_DA/"]
RUN dotnet restore "DemoWebApi/DemoWebApi.csproj"
COPY . .
WORKDIR "/src/DemoWebApi"
RUN dotnet build "DemoWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DemoWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DemoWebApi.dll"]