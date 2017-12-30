# RuleEngine Switchs #

This script requires RuleEngineService dependency

Typical scenario why I created this tool :
- Switch on the christmas tree depending on : 
   - we are at home (using an automated service personas)
   - but not the night (time based)
   - startup time depends on holiday or not (but time is dynamic depending on a Calendar Module)

- Reboot the FAI Box to get updates only once by month if we are at home

- Switch on a radiator/heater depending on we are at home and the room temperature is below a time based value

How to use :
Create a script with your rules (see samples below)

Add a RuleEngineSwitch module to a Group.
Configure the fields :
- Rule Name : The name of the rule as created in the switch
- Output : The switch/light module name that will be crontolled by rules

Then enable the RuleEngineSwitch module

The destination switch/light will be controlled by your rules


Sample C# Setup script to initialise some rules :

Create a new csharp script

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
