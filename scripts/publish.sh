flags='--runtime=linux-x64'

dotnet restore src/FlightBooking.sln $flags

dotnet publish src/FlightBooking.Gateway/FlightBooking.Gateway.csproj -c Release -o publish/FlightBookingGateway --no-restore $flags
dotnet publish src/FlightBooking.TicketService/FlightBooking.TicketService.csproj -c Release -o publish/TicketService --no-restore $flags
dotnet publish src/FlightBooking.FlightService/FlightBooking.FlightService.csproj -c Release -o publish/FlightService --no-restore $flags
dotnet publish src/FlightBooking.BonusService/FlightBooking.BonusService.csproj -c Release -o publish/BonusService --no-restore $flags
