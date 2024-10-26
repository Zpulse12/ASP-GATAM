if ! command -v "docker" >/dev/null 2>&1; then
    echo "docker not installed. Run SETUP script first."
else
    docker network create staging-proxy
    docker network create production-proxy
    docker compose -f ./docker-compose.yml up -d --build
fi