# This one needs to be ran after dotnet build/publish has happened
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS base
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
WORKDIR /app
COPY bin/Release/netcoreapp3.1/publish .
ENTRYPOINT ["dotnet", "Strife.Blazor.Server.dll"]

# For quick reference 
# docker build -t strife .
# docker run -p 8080:5000 strife