#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat


FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
USER 0
WORKDIR /app
RUN chown -R 1001:0 /app && chmod -R og+rwx /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080


FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY ["ScheduleService.csproj", "./"]
RUN dotnet restore "./ScheduleService.csproj"
COPY . .
RUN dotnet build "ScheduleService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScheduleService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScheduleService.dll"]
