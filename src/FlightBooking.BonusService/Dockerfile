FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 8050

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FlightBooking.BonusService/FlightBooking.BonusService.csproj", "FlightBooking.BonusService/"]
COPY ["FlightBooking.BonusService.Dto/FlightBooking.BonusService.Dto.csproj", "FlightBooking.BonusService.Dto/"]
RUN dotnet restore "FlightBooking.BonusService/FlightBooking.BonusService.csproj"

COPY . .
WORKDIR "/src/FlightBooking.BonusService"

FROM build AS publish
RUN dotnet publish "FlightBooking.BonusService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightBooking.BonusService.dll"]
