# RuleEngine Service #

This is a rule engine to be used in your own scripts

Sample C# Setup script to initialise some rules :

```csharp
  Thread.Sleep(2000);  // Ensure the service is registered before startup
  dynamic RuleEngineService = ProgramDynamicApi.Find("Service/Tools/ServiceRuleEngine/v1")("");

  /// this sample show a custom rule used to switch on/off christmas lights if :
  //  - we are at home (using ServicePersona)
  //  - depending of day of week and holidays dates this change the startup time

  // custom rule
  Func<dynamic, DayOfWeek, int, bool> EvaluateAHSChildSunTime = (rule, dow, it) => {
    var atHomeModule = Modules.WithName("ServicePersonas").Get();
    if (atHomeModule.Exists) {
      if (atHomeModule.Parameter("Sensor.AtHome").Value == "0" )
          return false;
    }
    
    var cal = Modules.WithName("Calendrier").Get();
  	if (cal.Exists) {
      // adjust rule startTime
      rule.TimeStart = int.Parse(cal.Parameter("HourSoleilStart").Value);
    }
    
    return RuleEngineService.BaseEvaluateRule(rule, dow, it);
  };
  
  // first we register the rule to the serviceRuleEngine
  RuleEngineService.Register("AHSwitch.SapinNoel");

  // then we describe the switch on rule
  // order of added rules is important
  // the first that match will be returned
  dynamic dayrule = new ExpandoObject();
  dayrule.TimeStart = 700;   // time are described as integers and are required if you use the default Rule (RuleEngineService.BaseEvaluateRule)
  dayrule.TimeEnd = 2200;    // they are easily readable/writable : 24 hours, 700 => 7h00, 2200 => 22h00
  dayrule.DaysOfWeek = null; // DaysOfWeek = null, all days will be used, samples with only some days : dayrule.DaysOfWeek = new DayOfWeek[] {DayOfWeek.Wednesday, DayOfWeek.Saturday, DayOfWeek.Sunday};
  dayrule.Evaluate = EvaluateAHSChildSunTime;  // the custom rule will ovveride the default rule
  dayrule.Value = 255;       // then you can add what you need to the rule : there we add a value that will be used to switch on the christmas lights.
  RuleEngineService.AddRule("AHSwitch.SapinNoel", dayrule);  // we append the rule to the RuleEngine, remember the order is important.
  
  // now we add as the last one, the default rule
  // in this case, the default rule will switch off the christmas lights
  dynamic defaultrule = new ExpandoObject();
  defaultrule.TimeStart = -1;
  defaultrule.TimeEnd = 2400;
  defaultrule.DaysOfWeek = null;
  defaultrule.Value = 0;
  // we do not define a custome Rule (Evaluate) so the default RuleEngineService.BaseEvaluateRule will be used
  RuleEngineService.AddRule("AHSwitch.SapinNoel", defaultrule);
  


  // this is another sample
  // will switch off for a minute the IAP box, because it receives software updates only when restarted
  Func<dynamic, DayOfWeek, int, bool> EvaluateAHSFirstSundayOfMonth = (rule, dow, it) => {
    var dtnow = DateTime.Now;
    if (dtnow.Day < 7)
      return false;
    
    var atHomeModule = Modules.WithName("ServicePersonas").Get();
    if (atHomeModule.Exists) {
      if (atHomeModule.Parameter("Sensor.AtHome").Value == "0" )
          return false;
    }
    return RuleEngineService.BaseEvaluateRule(rule, dow, it);
  };
  
  // the switch off rule
  RuleEngineService.Register("ComplexSchedule.RebootFreebox");
  dynamic rbtfreebox = new ExpandoObject();
  rbtfreebox.TimeStart = 500;
  rbtfreebox.TimeEnd = 501;
  rbtfreebox.DaysOfWeek = new DayOfWeek[] { DayOfWeek.Sunday };
  rbtfreebox.Evaluate = EvaluateAHSFirstSundayOfMonth;
  rbtfreebox.Value = 0;
  RuleEngineService.AddRule("ComplexSchedule.RebootFreebox", rbtfreebox);
  
  // the default rule (switch on)
  dynamic defaultrulefreebox = new ExpandoObject();
  defaultrulefreebox.TimeStart = -1;
  defaultrulefreebox.TimeEnd = 2400;
  defaultrulefreebox.DaysOfWeek = null;
  defaultrulefreebox.Value = 255;
  RuleEngineService.AddRule("ComplexSchedule.RebootFreebox", defaultrulefreebox);

```

Then to use the ruleengine here's a sample :


```csharp

  Thread.Sleep(2000);  // Ensure the service is registered before at startup


// do the job
while (Program.IsEnabled)
{
  dynamic RuleEngineService = ProgramDynamicApi.Find("Service/Tools/ServiceRuleEngine/v1")("");

  var destModule = Modules.WithName("PriseFreebox").Get();
  if (destModule.Exists) {
	dynamic rule = RuleEngineService.EvaluateRules("ComplexSchedule.RebootFreebox");  // here we evaluate all rules, the selected rule will be returned
	if (rule != null) {
		if ((rule.Value == 0) && (destModule.Parameter("Status.Level").Value == "1" )) {
			destModule.Off();
		} else if ((rule.Value > 0) && (destModule.Parameter("Status.Level").Value == "0" )) {
			destModule.On();
		}
	}
  }
  //
  Pause(30);
}

```