PROJECTNAME := $(shell basename "$(PWD)")
PID := /tmp/.$(PROJECTNAME).pid
DYNAMO_LOG_FILE := .dynamo/log.txt

.PHONY: default
default:
	@echo "Run make build or make test"

.PHONY: build
build:
	@echo "Building..."
	dotnet build

.PHONY: test
test:
	dotnet test

.PHONY: test_not_dynamo
test_not_dynamo:
	dotnet test --filter TestCategory!=DynamoTests

.PHONY: back
back:
	sam build
	sam local start-api --port 3001

.PHONY: front
front:
	(cd chameleon-webapp/ && npm start)

.PHONY: dynamo_server
dynamo_server:
	@echo "Running local dynamo server"
	# See: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Tools.CLI.html#Tools.CLI.UsingWithDDBLocal
	docker run -p 8000:8000 --name localtestdb amazon/dynamodb-local

.PHONY: start_dynamo
start_dynamo: stop_dynamo
	make dynamo_server >> $(DYNAMO_LOG_FILE) 2>&1 & echo $$! > $(PID)
	./.dynamo/create-table.sh

.PHONY: stop_dynamo
stop_dynamo:
	docker stop localtestdb
	docker rm localtestdb
	touch $(PID)
	kill -9 `cat $(PID)` 2> /dev/null || true
	rm $(PID)
	rm $(DYNAMO_LOG_FILE)
