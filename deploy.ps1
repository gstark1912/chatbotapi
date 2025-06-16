# deploy.ps1
git pull origin main
docker compose build api
docker compose restart api
