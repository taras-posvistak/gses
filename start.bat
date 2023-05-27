@echo off
copy /Y ".\gses\Dockerfile" ".\Dockerfile"
docker build . -t gses
docker run -d -p 5000:80 --rm --env-file .env gses