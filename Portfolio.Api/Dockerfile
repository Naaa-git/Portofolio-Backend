# Stage 1: build — pakai SDK lengkap, cuma dipakai pas build, dibuang nanti.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj dan restore dulu, SEBELUM copy sisa source code. Docker nge-cache
# tiap instruksi sebagai layer — selama csproj nggak berubah, layer restore ini
# dipakai ulang dari cache (skip redownload NuGet packages), walau kode .cs
# berubah tiap hari. Kalau langsung copy semuanya + restore dalam 1 langkah,
# cache-nya invalid setiap ada perubahan baris kode apapun.
COPY Portfolio.Api.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: runtime — image kecil, cuma bawa hasil publish, SDK-nya nggak ikut.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]
