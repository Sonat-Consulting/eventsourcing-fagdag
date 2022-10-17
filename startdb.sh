#!/bin/bash

docker-compose up -d

sleep 15

docker exec mongo1 ./scripts/rs-init.sh