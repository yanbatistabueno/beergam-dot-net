FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Beergam/Beergam.csproj", "Beergam/"]
RUN dotnet restore "Beergam/Beergam.csproj"
COPY . .
WORKDIR "/src/Beergam"
RUN dotnet build "./Beergam.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Beergam.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Beergam.dll"]

FROM build AS dev
WORKDIR /src/Beergam
# Install the EF Core CLI matching Pomelo 9.0.0 (EF Core 9) and add it to PATH
RUN dotnet tool install --global dotnet-ef --version 9.*
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://+:5000"]