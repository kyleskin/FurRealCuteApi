FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FurRealCute.Web.Api/FurRealCute.Web.Api.csproj", "FurRealCute.Web.Api/"]
RUN dotnet restore "FurRealCute.Web.Api/FurRealCute.Web.Api.csproj"
COPY . .
WORKDIR "/src/FurRealCute.Web.Api"
RUN dotnet build "FurRealCute.Web.Api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "FurRealCute.Web.Api.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FurRealCute.Web.Api.dll"]
