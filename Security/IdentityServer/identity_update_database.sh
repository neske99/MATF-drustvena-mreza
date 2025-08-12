#!/bin/bash

REPOSITORY_PATH=./IdentityServer.csproj

dotnet ef database update --project "$REPOSITORY_PATH"