COPY MailPosterAPI/*.csproj ./MailPosterAPI/
RUN dotnet restore

RUN dotnet add MailPosterAPI/MailPosterAPI.csproj package Npgsql.EntityFrameworkCore.PostgreSQL

COPY MailPosterAPI/. ./MailPosterAPI/

WORKDIR /app/MailPosterAPI
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 80
COPY --from=build /app/MailPosterAPI/out .
ENTRYPOINT ["dotnet", "MailPosterAPI.dll"]
