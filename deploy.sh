#!/bin/bash

cd /app
git pull origin main
docker compose build api
docker compose restart api
