# Web Push Notifications Tutorial

This is a tutorial and demo for web push notifications that work in modern web browsers.

## How to use

First, install all .NET dependencies via `dotnet restore`.

This demo uses an in-memory SQL database instance for storing push subscription info to send push updates at some other point in time. It also requires specifying a public and private key for identifying your server to the push service's server. These keys, known as VAPID public/private keys, can be generated and printed to the console when first executing the site. The site can be executed by running `dotnet run` which will start a server on `https://localhost:5001`. You'll need to populate those keys as environment variables and execute `dotnet run` again to ensure that push messages can be configured from your server.

You should set the environment variables mentioned above in your `appsettings.json` file as follows:

```json
{
  "Vapid": {
    "Subject": "mailto:email@outlook.com",
    "PublicKey": "YOUR_PUBLIC_KEY",
    "PrivateKey": "YOUR_PRIVATE_KEY"
  },
  "ConnectionStrings": {
    "Database": "Server=(localdb)\\mssqllocaldb;Database=PushDemoInMemoryDb;Trusted_Connection=True;ConnectRetryCount=0"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
