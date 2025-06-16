#!/bin/bash

git pull origin main
docker compose build api
docker compose restart api
