# SmartMessaging Service #

Smarten Messaging services, this service helps making integration of messaging services more flexible in your own scripts.

 Typical usage : 
- Send a pushbullet notification to two different users.
- Send a mail and a push notification when an event occur in your scripts.

Sample C# script to use the Service : (doorbell sample)

Initialisation : 
```csharp
dynamic pusbulletService = ProgramDynamicApi.Find("Service/Messaging/PushBullet/v1")("");
dynamic mailingService = ProgramDynamicApi.Find("Service/Messaging/Mail/v1")("");

dynamic smartMessagingService = ProgramDynamicApi.Find("Service/Messaging/SmartMessaging/v1")("");
smartMessagingService.Register("doorbell", pusbulletService, "your_pushbullet_api_key");
smartMessagingService.Register("doorbell", mailingService, "test@test.com");
```

then in your doorbell script :

```csharp
dynamic smartMessagingService = ProgramDynamicApi.Find("Service/Messaging/SmartMessaging/v1")("");
smartMessagingService.Send("Doorbell Information", "Someone made Ding/Dong", "doorbell");
```
