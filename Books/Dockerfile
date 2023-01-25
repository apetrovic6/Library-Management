FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY Books.csproj .
RUN dotnet restore Books.csproj
COPY . .
RUN dotnet build "Books.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Books.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Books.dll"]
