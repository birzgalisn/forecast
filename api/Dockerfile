# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /App

COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runner
WORKDIR /App

ENV ASPNETCORE_ENVIRONMENT=Production

RUN addgroup --system --gid 1001 dotnet
RUN adduser --system --uid 1001 --ingroup dotnet webapi

COPY --from=builder --chown=webapi:dotnet /App/out .

USER webapi

EXPOSE 4000/tcp

ENTRYPOINT ["dotnet", "api.dll"]
