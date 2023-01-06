FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FlightBooking.Gateway/FlightBooking.Gateway.csproj", "FlightBooking.Gateway/"]
COPY ["FlightBooking.BonusService.Dto/FlightBooking.BonusService.Dto.csproj", "FlightBooking.BonusService.Dto/"]
COPY ["FlightBooking.FlightService.Dto/FlightBooking.FlightService.Dto.csproj", "FlightBooking.FlightService.Dto/"]
COPY ["FlightBooking.TicketService.Dto/FlightBooking.TicketService.Dto.csproj", "FlightBooking.TicketService.Dto/"]
RUN dotnet restore "FlightBooking.Gateway/FlightBooking.Gateway.csproj"
COPY . .
WORKDIR "/src/FlightBooking.Gateway"

FROM build AS publish
RUN dotnet publish "FlightBooking.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightBooking.Gateway.dll"]
