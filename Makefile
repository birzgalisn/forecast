DOCKER_COMPOSE := docker compose --env-file .env

BUILD := build --parallel
UP := up
REMOVE := down -v --rmi all --remove-orphans
PULL := pull

define docker_compose_cmd
$(DOCKER_COMPOSE) -f .docker/$(1)/docker-compose.yaml
endef

ENVS := dev staging prod

$(ENVS:%=build-%):
	$(call docker_compose_cmd,$*) $(BUILD)

$(ENVS:%=pull-%):
	$(call docker_compose_cmd,$*) $(PULL)

$(ENVS:%=start-%):
	$(call docker_compose_cmd,$*) $(UP)

$(ENVS:%=remove-%):
	$(call docker_compose_cmd,$*) $(REMOVE)

remove-dangling:
	docker image prune -f

remove-all: $(ENVS:%=remove-%)

.PHONY: $(ENVS:%=build-%) $(ENVS:%=pull-%) $(ENVS:%=start-%) $(ENVS:%=remove-%) remove-dangling remove-all
