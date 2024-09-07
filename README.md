# Focal Movies API

Focal Movies API is a movie database API built with ASP.NET Core (.NET 8) and Docker. Follow these steps to get it up and running on your local machine.

## Getting Started

### Prerequisites

Ensure you have the following installed:

- [Docker](https://docs.docker.com/get-docker/)

### Build the Docker Image

Navigate to the root of the project directory and build the Docker image using:

```bash
docker build -t focal-movies-api -f .\Focal.Movies.API\Dockerfile .
```
start spawn a container using created image name and bind it to a free port on your machine

```bash
docker run -p 5021:8080 --name focal-movies-api-container focal-movies-api
```

### Access the API

navigate to the mapped local port (5021, change it if you need to) and use swagger endpoint

- [Swagger](http://localhost:5021/swagger)

Now you're good to go 😉

Start using the api from swagger