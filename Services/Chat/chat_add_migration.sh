#!/bin/bash

MIGRATION_NAME=$1
REPOSITORY_PATH=./Services/Chat/Chat.Repository/Chat.Repository.csproj
STARTUP_PATH=./Services/Chat/Chat.API/Chat.API.csproj

dotnet ef migrations add "$MIGRATION_NAME" --project "$REPOSITORY_PATH" --startup-project "$STARTUP_PATH"