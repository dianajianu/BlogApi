#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Blog.Api.Test/Blog.Api.Test.csproj", "Blog.Api.Test/"]
COPY ["Blog.Api/Blog.Api.csproj", "Blog.Api/"]
COPY ["Blog.Api.Models/Blog.Api.Dto.csproj", "Blog.Api.Models/"]
RUN dotnet restore "Blog.Api.Test/Blog.Api.Test.csproj"
COPY . .
WORKDIR "/src/Blog.Api.Test"
RUN dotnet build "Blog.Api.Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blog.Api.Test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.Api.Test.dll"]