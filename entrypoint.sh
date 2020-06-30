#!/bin/bash

set -e 
export PATH="$PATH:/root/.dotnet/tools"
dotnet tool install --global dotnet-ef
run_cmd="dotnet run --server.urls http://*:5001"  
until dotnet ef database update --project ./api/film-api/FilmApi.csproj; do 
>&2 echo "SQL Server is starting up" 
sleep 1 
done  
>&2 echo "SQL Server is up - executing command" 
exec $run_cmd 
