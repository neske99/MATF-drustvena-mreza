FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Relations/Relations.GRPC/Relations.GRPC.csproj", "Services/Relations/Relations.GRPC/"]
COPY ["Services/Relations/Relations.Common/Relations.Common.csproj", "Services/Relations/Relations.Common/"]
RUN dotnet restore "Services/Relations/Relations.GRPC/Relations.GRPC.csproj"
COPY . .
WORKDIR "/src/Services/Relations/Relations.GRPC"
RUN dotnet build "Relations.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Relations.GRPC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Relations.GRPC.dll"]
