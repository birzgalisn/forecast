DOCKER_COMPOSE := docker compose --env-file .env

DEV := $(DOCKER_COMPOSE) -f .docker/dev/docker-compose.yaml

BUILD := build --parallel
UP := up
REMOVE := down -v --rmi all --remove-orphans

build-dev:
	$(DEV) $(BUILD)

start-dev:
	$(DEV) $(UP)

remove-dev:
	$(DEV) $(REMOVE)
