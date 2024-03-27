DOCKER_COMPOSE := docker compose --env-file .env

DEV := $(DOCKER_COMPOSE) -f .docker/dev/docker-compose.yaml
STAGING := $(DOCKER_COMPOSE) -f .docker/staging/docker-compose.yaml
PROD := $(DOCKER_COMPOSE) -f .docker/prod/docker-compose.yaml

BUILD := build --parallel
UP := up
REMOVE := down -v --rmi all --remove-orphans
PULL := pull

build-dev:
	$(DEV) $(BUILD)

pull-dev:
	$(DEV) $(PULL)

start-dev:
	$(DEV) $(UP)

remove-dev:
	$(DEV) $(REMOVE)

build-staging:
	$(STAGING) $(BUILD)

pull-staging:
	$(STAGING) $(PULL)

start-staging:
	$(STAGING) $(UP)

remove-staging:
	$(STAGING) $(REMOVE)

pull-prod:
	$(PROD) $(PULL)

start-prod:
	$(PROD) $(UP)

remove-prod:
	$(PROD) $(REMOVE)

remove-dangling:
	docker image prune -f

remove-all: remove-dev remove-staging remove-prod
