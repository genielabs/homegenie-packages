# Pushbullet Service #

Give a way to use Pushbullet as a messaging service in your own HomeGenie scripts

Pushbullet is Notification (Push) service for your devices. see [https://www.pushbullet.com/](https://www.pushbullet.com/) for more informations.

Sample C# script to use the Service :

```csharp
dynamic pushbulletService = ProgramDynamicApi.Find("Service/Messaging/PushBullet/v1")("");
pushbulletService.Send("test Title", "This is a test Message", "your_pushbullet_api_key");
```

With your pushbullet account you can get your api key (or access token) from here :

[https://www.pushbullet.com/#settings/account](https://www.pushbullet.com/#settings/account)
