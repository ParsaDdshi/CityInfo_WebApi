
# City Info API
 This project implemented with asp.net core web api, api returns cities and their point of intersets and you can do CRUD operations with it. Documentation implemented with swagger.

> [!WARNING]  
The information in the database is not complete, so you must enter your data.


## Tech Stack
 Asp.net core Web Api, Ef core, Sqlite

## Installation
 After installing EfCore packages, you need to run the following commands in Package Manager Consloe.

```bash
  Add-Migration Init-Database
```

```bash
  Update-Database
```

## Getting Token

after runnig the project, for getting token,  send values { "username":"admin", "password":"admin" } to route "/api/authentication/authenticate".

## IDE USED
- Visual Studio 2022

## Developer

- [@Parsa Dadashi](https://github.com/ParsaDdshi)

