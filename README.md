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

## Run

```bash
cd GamesApi
dotnet run
```

The API starts on a local URL (for example, `http://localhost:5000` or `https://localhost:7000`).

## Endpoint

### `GET /games`

Returns 5 in-memory games with fields:

- `id`
- `title`
- `genre`
- `platform`

Example:

```bash
curl http://localhost:5000/games
```

Example response:

```json
[
  {
    "id": 1,
    "title": "The Legend of Zelda: Breath of the Wild",
    "genre": "Action-adventure",
    "platform": "Nintendo Switch"
  }
]
```
