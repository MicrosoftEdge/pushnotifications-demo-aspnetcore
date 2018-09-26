# Web Push Notifications Tutorial

This is a tutorial and demo for web push notifications that work in modern web browsers. Frontend is written in vanilla JavaScript, while the backend is using the ASP.NET Core 2.1 framework.

> If you are searching the node.js version of this sample, you can find it [here](https://github.com/MicrosoftEdge/pushnotifications-demo).

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

## Key components of the sample

The following files contain code that's related to generating VAPID keys, registering a push subscription and sending push notifications.

### ASP.NET Core backend

- [`appsettings.json`](/PushnotificationsDemo/appsettings.json) Contains VAPID keys and the database connection string.
- [`Startup.cs`](/PushnotificationsDemo/Startup.cs) Configures the app and the services it uses, including the database connection.
- [`PushController.cs`](/PushnotificationsDemo/Controllers/PushController.cs) Contains the API endpoints.
- [`PushService.cs`](/PushnotificationsDemo/Services/PushService.cs) Contains the Push service which is used to manage saving subscriptions to the database and sending push notifications.

### Frontend

- [`Index.cshtml`](/PushnotificationsDemo/Views/Home/Index.cshtml) Contains the sample's UI.
- [`service-worker.js`](/PushnotificationsDemo/wwwroot/service-worker.js) Contains the sample's service worker which gets registered and will manage the incoming push notifications.
- [`script.js`](/PushnotificationsDemo/wwwroot/js/script.js) Runs after DOM is loaded and contains methods for service worker and push subscription registration.
- [`util.js`](/PushnotificationsDemo/wwwroot/js/util.js) Contains methods for push subscription management.

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Questions and comments

We'd love to get your feedback about the Microsoft Graph Connect Sample for ASP.NET Core. You can send your questions and suggestions to us in the [Issues](https://github.com/MicrosoftEdge/pushnotifications-demo-aspnetcore/issues) section of this repository.

Questions about Microsoft Edge in general should be posted to [Stack Overflow](https://stackoverflow.com/questions/tagged/microsoft-edge). Make sure that your questions or comments are tagged with _[microsoft-edge]_.

You can suggest changes for Microsoft Edge on [UserVoice](https://wpdev.uservoice.com/forums/257854-microsoft-edge-developer).

## Copyright

Copyright (c) 2018 Microsoft. All rights reserved.
