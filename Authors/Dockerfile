FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Authors/Authors.csproj", "Authors/"]
RUN dotnet restore "Authors/Authors.csproj"
COPY . .
WORKDIR "/src/Authors"
RUN dotnet build "Authors.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authors.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authors.dll"]
