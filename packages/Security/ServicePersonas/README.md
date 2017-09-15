# Personas Service #

Integrate in homegenie people in your house, in order to create rules based on personas presence.

Sample C# Setup script to use the Service :

```csharp
dynamic personaService = ProgramDynamicApi.Find("Service/Security/Personas/v1")("");

dynamic dady = new ExpandoObject();
dady.Name = "Dady";
dady.Room = "Chambre1";
dady.FollowGlobal = false;

personaService.Personas.Add(dady);

dynamic son = new ExpandoObject();
son.Name = "Alex";
son.Room = "ChambreEtageDroit";
son.FollowGlobal = true;

personaService.Personas.Add(son);

```

then you can use them like this :

```csharp
dynamic personaService = ProgramDynamicApi.Find("Service/Security/Personas/v1")("");

if (personaService.getState("Dady") == personaService.StateAtHome) {
	// do some stuff
}
```

or register a handler :

```csharp
dynamic personaService = ProgramDynamicApi.Find("Service/Security/Personas/v1")("");

personaService.RegisterEventHandler("Dady", (persona, state) => {
	if (state == personaService.StateAtHome) {
		// do some stuff
	}
})

```




or using a classic homegenie event :

```csharp

When.ModuleParameterChanged( (module, parameter) => 
{
      ...
	  
	  if (parameter.Name == "persona.Dady") {
	       // do some stuff
	  }

}
```
