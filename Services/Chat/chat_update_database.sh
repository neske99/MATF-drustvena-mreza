#!/bin/bash

REPOSITORY_PATH=./Chat.Repository/Chat.Repository.csproj
STARTUP_PATH=./Chat.API/Chat.API.csproj

dotnet ef database update --project "$REPOSITORY_PATH" --startup-project "$STARTUP_PATH"