FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 8070

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FlightBooking.TicketService/FlightBooking.TicketService.csproj", "FlightBooking.TicketService/"]
COPY ["FlightBooking.TicketService.Dto/FlightBooking.TicketService.Dto.csproj", "FlightBooking.TicketService.Dto/"]
RUN dotnet restore "FlightBooking.TicketService/FlightBooking.TicketService.csproj"
COPY . .
WORKDIR "/src/FlightBooking.TicketService"

FROM build AS publish
RUN dotnet publish "FlightBooking.TicketService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightBooking.TicketService.dll"]
