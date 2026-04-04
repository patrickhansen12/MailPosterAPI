FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# 🔥 FIX: Deaktiver GSSAPI/Kerberos (undgår "libgssapi_krb5.so.2: cannot open")
ENV Npgsql__DisableGssapi=true

# Kopier solution fil
COPY *.sln .

# Kopier projektfil og gendan dependencies
COPY MailPosterAPI/*.csproj ./MailPosterAPI/
RUN dotnet restore

# Installer PostgreSQL pakke
RUN dotnet add MailPosterAPI/MailPosterAPI.csproj package Npgsql.EntityFrameworkCore.PostgreSQL

# Kopier resten af koden
COPY MailPosterAPI/. ./MailPosterAPI/

# Byg og publish
WORKDIR /app/MailPosterAPI
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 80

# 🔥 FIX: Også i runtime-imageet for en sikkerheds skyld
ENV Npgsql__DisableGssapi=true

COPY --from=build /app/MailPosterAPI/out .
ENTRYPOINT ["dotnet", "MailPosterAPI.dll"]
