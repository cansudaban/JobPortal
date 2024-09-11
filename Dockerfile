#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["JobPortal.WebApi/JobPortal.WebApi.csproj", "JobPortal.WebApi/"]
COPY ["JobPortal.Business/JobPortal.Business.csproj", "JobPortal.Business/"]
COPY ["JobPortal.Data/JobPortal.Data.csproj", "JobPortal.Data/"]
COPY ["JobPortal.Common/JobPortal.Common.csproj", "JobPortal.Common/"]
RUN dotnet restore "JobPortal.WebApi/JobPortal.WebApi.csproj"
COPY . .
WORKDIR "/src/JobPortal.WebApi"
RUN dotnet build "JobPortal.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JobPortal.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobPortal.WebApi.dll"]