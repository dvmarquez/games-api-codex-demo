# Games API Codex Demo

A very small ASP.NET Core minimal API that serves an in-memory list of video games.

## Requirements

- .NET SDK 10.0+ (this project targets `net10.0`)

> If .NET 10 is not available on your machine, install the latest available .NET SDK and update `TargetFramework` in `GamesApi/GamesApi.csproj` accordingly.

## Setup

```bash
git clone <your-repo-url>
cd games-api-codex-demo
```

## Build

```bash
dotnet build GamesApi/GamesApi.csproj
```

## Run the API

```bash
cd GamesApi
dotnet run
```

By default, ASP.NET Core prints URLs like `http://localhost:5000` or `https://localhost:7000` in the console.

## Swagger / OpenAPI

Swagger UI is enabled to make local testing easier.

- Swagger JSON: `http://localhost:5000/swagger/v1/swagger.json`
- Swagger UI: `http://localhost:5000/swagger`

> If your app starts on a different port, replace `5000` with that port.

## Test the API with curl

### `GET /games`

```bash
curl -i http://localhost:5000/games
```

Sample response:

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

[
  {
    "id": 1,
    "title": "The Legend of Zelda: Breath of the Wild",
    "genre": "Action-adventure",
    "platform": "Nintendo Switch"
  }
]
```

### `GET /games/1`

```bash
curl -i http://localhost:5000/games/1
```

Sample response:

```http
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8

{
  "id": 1,
  "title": "The Legend of Zelda: Breath of the Wild",
  "genre": "Action-adventure",
  "platform": "Nintendo Switch"
}
```

### `GET /games/999`

```bash
curl -i http://localhost:5000/games/999
```

Sample response:

```http
HTTP/1.1 404 Not Found
```

### `POST /games`

```bash
curl -i http://localhost:5000/games \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Celeste",
    "genre": "Platformer",
    "platform": "PC"
  }'
```

Sample response:

```http
HTTP/1.1 201 Created
Content-Type: application/json; charset=utf-8
Location: /games/6

{
  "id": 6,
  "title": "Celeste",
  "genre": "Platformer",
  "platform": "PC"
}
```

Sample validation error response:

```http
HTTP/1.1 400 Bad Request
Content-Type: application/problem+json; charset=utf-8

{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Title": [
      "'Title' must not be empty."
    ]
  }
}
```

## Run tests

```bash
dotnet test GamesApi/GamesApi.csproj
```

This project currently has no test project, so this command verifies restore/build and reports zero tests.
