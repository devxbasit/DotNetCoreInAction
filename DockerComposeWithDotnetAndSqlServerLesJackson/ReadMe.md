# Docker Commands

docker build -t devxbasit/colorapi .
docker push devxbasit/colorapi
docker-compose up

docker run -p 127.0.0.1:8080:8080 -d devxbasit/colorapi
