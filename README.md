# Football Players API

The API offers information on current players from the English Premier League.  
Deployed as an Azure Web App, it uses Azure SQL Database for data storage.

## Live Demo
https://football-players.azurewebsites.net/

# Endpoints documentation
 
## Players

| Endpoint                 | Description                                                |
|--------------------------|------------------------------------------------------------|
| **Get Players**          |                                                            |
| **URL**: `/api/players`   | **Method**: `GET`<br>**Parameters**: `paginationFilters`, `playerFilter`<br>**Response**:<br> `200 OK` with list of players<br>`404 Not Found` if no data is found in the database |
| **Get Player by ID**     |                                                            |
| **URL**: `/api/players/{id}` | **Method**: `GET`<br>**Parameters**: `id` (int): Unique identifier of the player<br>**Response**:<br>`200 OK` with player details<br>`404 Not Found` if player with the given ID doesn't exist in the database |

### Pagination Headers

The `GetPlayers` endpoint supports pagination. The following pagination headers will be included in the response:

- `CurrentPage`: Current page number (1 by default).
- `PageSize`: Number of items per page (10 by default).
- `TotalRecords`: Total number of records.
- `TotalPages`: Total number of pages.
- `HasPrevious`: Indicates (true) if there's a previous page.
- `HasNext`: Indicates (true) if there's a next page.

```
Pagination: {"CurrentPage":1,"PageSize":5,"TotalRecords":5,"TotalPages":1,"HasPrevious":false,"HasNext":false}
```

### Player Filter

The `playerFilter` parameter allows you to filter players based on specific criteria. Available filter options include:

- `country`: Filter players by country.
- `club`: Filter players by club.
- `position`: Filter players by position.

### Example Usage

```http
GET https://football-players.online/api/players
GET https://football-players.online/api/players?Club=Everton
GET https://football-players.online/api/players?PageNumber=2&PageSize=5&Country=England&Position=Forward
GET https://football-players.online/api/players/182
```

### Return format

```json
{
  "playerId": 182,
  "firstName": "Mike",
  "lastName": "Trésor",
  "imgSrc": "https://resources.premierleague.com/premierleague/photos/players/250x250/p437748.png",
  "dateOfBirth": "1999-05-28",
  "club": {
    "clubId": 10,
    "name": "Burnley",
    "badgeSrc": "https://resources.premierleague.com/premierleague/badges/t90.png"
  },
  "nationality": {
    "nationalityId": 6,
    "country": "Belgium",
    "flagSrc": "https://resources.premierleague.com/premierleague/flags/BE.png"
  },
  "position": {
    "positionId": 4,
    "name": "Forward"
  }
}
```

## Clubs

| Endpoint               | Description                                                |
|------------------------|------------------------------------------------------------|
| **Get Clubs**          |                                                            |
| **URL**: `/api/clubs`  | **Method**: `GET`<br>**Response**:<br> `200 OK` with list of clubs<br>`404 Not Found` if no data is found in the database |
| **Get Club by ID**     |                                                            |
| **URL**: `/api/clubs/{id}` | **Method**: `GET`<br>**Parameters**: `id` (int): Unique identifier of the club<br>**Response**:<br> `200 OK` with club details<br>`404 Not Found` if club with the given ID doesn't exist in the database |

### Example Usage

```http
GET https://football-players.online/api/clubs
GET https://football-players.online/api/clubs/24
```
### Return format

```json
{
  "clubId": 24,
  "name": "Manchester United",
  "badgeSrc": "https://resources.premierleague.com/premierleague/badges/t1.png"
}
```

## Nationalities

| Endpoint                    | Description                                                |
|-----------------------------|------------------------------------------------------------|
| **Get Nationalities**       |                                                            |
| **URL**: `/api/nationalities` | **Method**: `GET`<br>**Response**:<br> `200 OK` with list of nationalities<br>`404 Not Found` if no data is found in the database |
| **Get Nationality by ID**   |                                                            |
| **URL**: `/api/nationalities/{id}` | **Method**: `GET`<br>**Parameters**: `id` (int): Unique identifier of the nationality<br>**Response**:<br> `200 OK` with nationality details<br>`404 Not Found` if nationality with the given ID doesn't exist in the database |

### Example Usage

```http
GET https://football-players.online/api/nationalities
GET https://football-players.online/api/nationalities/3
```
### Return format

```json
{
  "nationalityId": 3,
  "country": "Argentina",
  "flagSrc": "https://resources.premierleague.com/premierleague/flags/AR.png"
}
```
 
## Positions

| Endpoint                | Description                                              |
|-------------------------|----------------------------------------------------------|
| **Get Positions**       |                                                          |
| **URL**: `/api/positions` | **Method**: `GET`<br>**Response**:<br> `200 OK` with list of positions<br>`404 Not Found` if no positions are found in the database |
| **Get Position by ID**  |                                                          |
| **URL**: `/api/positions/{id}` | **Method**: `GET`<br>**Parameters**: `id` (int): Unique identifier of the position<br>**Response**:<br> `200 OK` with position details<br>`404 Not Found` if position with the given ID doesn't exist in the database |

### Example Usage

```http
GET https://football-players.online/api/positions
GET https://football-players.online/api/positions/1
```

### Return format

```json
{
  "positionId": 1,
  "name": "Goalkeeper"
}
```
