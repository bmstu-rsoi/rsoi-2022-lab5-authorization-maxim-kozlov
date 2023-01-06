version=1.0

cd ./src
docker buildx build --push -t maxkozlov/rsoi-api-gateway:$version    --platform=linux/amd64 -f ./FlightBooking.Gateway/Dockerfile .
docker buildx build --push -t maxkozlov/rsoi-bonus-service:$version --platform=linux/amd64 -f ./FlightBooking.BonusService/Dockerfile .
docker buildx build --push -t maxkozlov/rsoi-flight-service:$version --platform=linux/amd64 -f ./FlightBooking.FlightService/Dockerfile .
docker buildx build --push -t maxkozlov/rsoi-ticket-service:$version  --platform=linux/amd64 -f ./FlightBooking.TicketService/Dockerfile .