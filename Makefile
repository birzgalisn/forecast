DOCKER_COMPOSE_DEV := docker compose -f ./docker-compose.dev.yml
DOCKER_COMPOSE_PROD := docker compose -f ./docker-compose.prod.yml

CLEAN_UP := docker image prune -a --force && docker volume prune --force
BUILD := build --no-cache --pull --parallel && $(CLEAN_UP)
UP := up
STOP := stop
REMOVE := down --rmi all --remove-orphans && $(CLEAN_UP)

build-dev:
	$(DOCKER_COMPOSE_DEV) $(BUILD)

dev:
	$(DOCKER_COMPOSE_DEV) $(UP)

remove-dev:
	$(DOCKER_COMPOSE_DEV) $(REMOVE)

build-prod:
	$(DOCKER_COMPOSE_PROD) $(BUILD)

prod:
	$(DOCKER_COMPOSE_PROD) $(UP) -d

stop-prod:
	$(DOCKER_COMPOSE_PROD) $(STOP)

remove-prod:
	$(DOCKER_COMPOSE_PROD) $(REMOVE)
