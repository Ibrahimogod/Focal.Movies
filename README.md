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
