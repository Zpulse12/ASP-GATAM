if ! command -v "docker" >/dev/null 2>&1; then
    echo "docker not installed. Run SETUP script first."
else
    docker network create --driver bridge proxy
    docker compose --env-file ./.env -f ./docker-compose.yml up -d --build
fi