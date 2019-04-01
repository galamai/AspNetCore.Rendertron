FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY samples/WebApplication/WebApplication.csproj samples/WebApplication/
COPY src/AspNetCore.Rendertron/AspNetCore.Rendertron.csproj src/AspNetCore.Rendertron/
RUN dotnet restore samples/WebApplication/WebApplication.csproj
COPY . .
WORKDIR /src/samples/WebApplication
RUN dotnet build WebApplication.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebApplication.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApplication.dll"]
