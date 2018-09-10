#!/bin/sh
cd ./apps/$1
dotnet run $1.dll
