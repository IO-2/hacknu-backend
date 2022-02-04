# Connect app backend

## Routes
![alt-text](./image.png)

## Add configuration file

Configuration file have to be as follows:

```json
{
  "ConnectionStrings": {
    "Default": "Host={HOST_NAME};Database={DB_NAME};Username={DB_USERNAME};Password={DB_USERNAME_PASSWORD}"
  },
  "JwtOptions": {
    "Secret": "{JSON_SECRET}"
  }
}
```

It can be created as appSettings.json, or as [project secret](https://docs.microsoft.com/en-gb/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows)
