#!/bin/bash

MIGRATION_NAME=$1
REPOSITORY_PATH=./IdentityServer.csproj

dotnet ef migrations add --project "$REPOSITORY_PATH"