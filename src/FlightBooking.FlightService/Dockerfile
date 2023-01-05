FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 8060

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FlightBooking.FlightService/FlightBooking.FlightService.csproj", "FlightBooking.FlightService/"]
COPY ["FlightBooking.FlightService.Dto/FlightBooking.FlightService.Dto.csproj", "FlightBooking.FlightService.Dto/"]
RUN dotnet restore "FlightBooking.FlightService/FlightBooking.FlightService.csproj"

COPY . .
WORKDIR "/src/FlightBooking.FlightService"

FROM build AS publish
RUN dotnet publish "FlightBooking.FlightService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightBooking.FlightService.dll"]
