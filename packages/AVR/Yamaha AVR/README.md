# Yamaha AV Receiver

I have several scenes which are based on knowing the details of my Yamaha audio/video receivers. For example, my motion-based lights are told not to turn off if the AVR in same room is on, as that means someone is still in there, just not moving very much. In the theater, I have modes for Xbox, movies, TV and party mode, which drive lights and TVs connected via Z-Wave, and another Yamaha receiver.

To that end, I've created a farly fully-featured Yamaha AVR app with widget. It currently will read and control the main zone power, volume, input and DSP.

# Program Examples

Turn power on/off: 

    YamahaAVR.Command("Control.Off").Set(" ");

Check DSP Status: 

    if (YamahaAVR.Parameter("Status.DSP").Value == "Action Game") 
    {
        /* ... */ 
    }

I do this to synchronize the volume between my two AVRs in the theater:

    if (parameter.Is("Status.Volume") && basementAvr2.Parameter("Status.Power").Value == "On")
    {
        basementAvr2.Command("Control.Volume").Set(((double.Parse(parameter.Value) * 10) + 120).ToString());
    }


