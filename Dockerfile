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

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/ .
COPY nginx.conf /etc/nginx/nginx.conf