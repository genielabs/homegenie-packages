# Mail Service #

There's allready some other way to use mail within homegenie, the main value of this Service is : it can be easily integrated in the SmartMessaging Service

Sample C# script to use the Service :

```csharp
dynamic mailService = ProgramDynamicApi.Find("Service/Messaging/Mail/v1")("");
mailService.Send("test", "This is a test Message", "destaddress@somedomain.com");
```

You'll need to configure your SMTP server address, port... on this Program to be able to use it.