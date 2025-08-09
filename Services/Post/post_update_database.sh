#!/bin/bash

REPOSITORY_PATH=./Post.Infrastructure/Post.Infrastructure.csproj
STARTUP_PATH=./Post.API/Post.API.csproj

dotnet ef database update --project "$REPOSITORY_PATH" --startup-project "$STARTUP_PATH"