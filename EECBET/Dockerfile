# 使用 .NET 9.0 運行時映像
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# 使用 .NET 9.0 SDK 映像進行建置
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 複製 csproj 並還原相依套件
COPY ["EECBET.csproj", "./"]
RUN dotnet restore "EECBET.csproj"

# 複製所有檔案並建置
COPY . .
RUN dotnet build "EECBET.csproj" -c Release -o /app/build

# 發佈應用程式
FROM build AS publish
RUN dotnet publish "EECBET.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 最終映像
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EECBET.dll"]