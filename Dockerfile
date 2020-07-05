FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Strife.Blazor/Server/Strife.Blazor.Server.csproj", "src/Strife.Blazor/Server/"]
COPY ["src/Strife.Blazor/Client/Strife.Blazor.Client.csproj", "src/Strife.Blazor/Client/"]
COPY ["src/Strife.Blazor/Shared/Strife.Blazor.Shared.csproj", "src/Strife.Blazor/Shared/"]
RUN dotnet restore "src/Strife.Blazor/Server/Strife.Blazor.Server.csproj"
COPY . .
WORKDIR "/src/src/Strife.Blazor/Server"
RUN dotnet build "Strife.Blazor.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Strife.Blazor.Server.csproj" -c Release -o /app/publish

FROM publish AS final
ENV ASPNETCORE_URLS=http://+:5000;https://+:5001
EXPOSE 5000
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish/ .
ENTRYPOINT ["dotnet", "Strife.Blazor.Server.dll"]


# For quick reference 
# docker build -t strife .
# docker run -p 8080:5000 -p 8081:5001 strife