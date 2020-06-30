FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY film-api/FilmApi.csproj ./api/film-api/
RUN ["dotnet", "restore", "./api/film-api/FilmApi.csproj"]


COPY film-api.sln ./api/
COPY film-api/ ./api/film-api/

RUN ["dotnet", "build", "./api/film-api/FilmApi.csproj"]

EXPOSE  5001/tcp

COPY entrypoint.sh .
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh


# WORKDIR /app/api/film-api
# ENTRYPOINT ["dotnet", "watch", "run"]