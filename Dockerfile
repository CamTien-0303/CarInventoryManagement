# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file và restore dependencies
COPY phamthicamtien/phamthicamtien.csproj phamthicamtien/
RUN dotnet restore phamthicamtien/phamthicamtien.csproj

# Copy toàn bộ source code
COPY phamthicamtien/ phamthicamtien/

# Build và publish
WORKDIR /src/phamthicamtien
RUN dotnet publish phamthicamtien.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render sẽ set biến PORT, app lắng nghe trên 0.0.0.0
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "phamthicamtien.dll"]
