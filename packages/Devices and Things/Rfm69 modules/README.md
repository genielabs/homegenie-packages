# Control 'Arduino clones with a transceiver' (EDUCATION version)

With this package you can control any type of arduino clones with a build in transceiver module to remote control a bunch of actors for different requests   
I.e. use Arduino clone **Moteino** or **MoteinoMEGA** and others to control remote Rfm69 driven switches.  
This app is written as a full functional **EDUCATION** version to help others understanding the full functionality of HomeGenie and the new widget style V2.

This packages consists of a HGX program (C#) and a widget (V2 sytle). THe program acts as the RESTFull service container for all requests from assigned widget   
THe JavaScript widget offers extended functionalities like: 
+ free configuration of Rfm69 nodes, pins and addresses 
+ separate log file for all interactions
+ Graph to display all switch times with filterable content
+ icon file upload functionality
+ selector for on/off icons

## Instructions

Install the package with app and widget.
The package includes also some images and language files for English and German (others could be easily added)

*You need to install the additional js package 'FileSaver.js' into the /usr/local/bin/homegenie/html/js path* 
See <a href="https://github.com/eligrey/FileSaver.js/"> FileSaver download page</a>

Start 'Rfm Switch Program'HGX programm   
the program is 'preconfigured' and uses the following parameters

+ Rfm.NetId: 			7  		- Network ID for all remote devices
+ Rfm.Port:  			/dev/ttyUSB0 	- Serial port of HomeGenie server to connect the Arduino clone board which acts as the central tranceiver
+ Rfm.Baudrate:			19200		- Serial baud rate (19200)
+ Rfm.Interval:			5		- Interval to send commands to clone
+ Rfm.UseQueue:			True		- Use command queue to collect command and send them in bulk every interval
+ Rfm.QueueSize:			5		- How many commands should be queued before sending
+ Rfm.VirtualModulesCount:	5		- Number of virtual modules
+ Rfm.UseLog:			On		- Use debug logging
+ Rfm.LogPath:			/usr/local/...	- Path of log file

Configure a prefered group and select  'Rfm switch' modules

The module widget supports the following module parameters:

+ Node:				address of RFM69 module to send commands to
+ Boardtype:		Moteino or MoteinoMEGA, **could be easily modified in HGX app**
+ Pin:				Available pins of selected board, **could be easily modified in HGX app**
+ On Icon:			Select an ON icon out of server side image path
+ Off Icon:			Select an OFF icon out of server side image path
+ Support for uploading PNG images to server path
	
and you can directly select a duration time and switch device on or off
+ switch Duration (1 min, 5 min, 10 min, 15 min, 30 min, 45 min, 60 min predefined) 
+ ON / Off slider

### Hardware needs

Any serial connected microcontroller driven device with an Rfm69 transceiver chip as the Gateway device (i used Moteino and MoteinoMEGA from https://lowpowerlab.com/
multiple Rfm69 transceiver driven devices as actors or sensor (sensor part i not included in the example)

See <a href="https://lowpowerlab.com/category/moteino/">detailed instructions of Moteino here</a>.   

### Board and pin configuration
---
```js
var boards = new[] { 
  new { 
    name = "MOTEINO", 		
    pins = new[] { 	"A0","A1","A2","A3","A4","A5","A6", "A7", "D0","D1","D2","D3","D4","D5","D6","D7","D8","D9","D10","D11","D12","D13","D14","D15","D16","D17","D18","D19",
    				"PB0","PB1","PB2","PB3","PB4","PB5","PC0","PC1","PC2","PC3","PC4","PC5", "PD0","PD1","PD2","PD3","PD4","PD5","PD6", "PD7" },
    cmd = new[]  {  "ON","OFF","TEST","GET"}},
  new { 
    name = "MOTEINOMEGA", 	
    pins = new[] {	"A0","A1","A2","A3","A4","A5","A6", "A7", "D0","D1","D2","D3","D4","D5","D6","D7","D8","D9","D10","D11","D12","D13","D14","D15","D16","D17","D18","D19",
                    "D20","D21","D22","D23","D24","D25","D26","D27","D28","D29","D30","D31","PA0","PA1","PA2","PA3","PA4","PA5","PA6","PA7","PB0","PB1","PB2","PB3","PB4",
                    "PB5","PB6","PB7","PC0","PC1","PC2","PC3","PC4","PC5","PC6","PC7","PD0","PD1","PD2","PD3","PD4","PD5","PD6" },
    cmd = new[]  {  "ON","OFF","TEST","GET"}}
	};  
```

### Interface structure
---
```js
Func<dynamic, bool> SendCommand = (dynamic dyn) => {
  
  string message = "";
  if (useJsonMessage == false) {
    message += "TRFM" + 			// Send RFM Command type 	length: 4
    ",N:" + dyn.Node.PadLeft(3,'0') + 		// Send node number		length: 6
    ",P:" + dyn.Port.PadRight(3,' ') + 		// Send port name		length: 6
    ",C:" + dyn.Command.PadRight(10,' ') + 	// Send command string		length: 13
    ",D:" + dyn.Duration.PadLeft(3,'0') +	// Send duration		length: 6
    "\n";					// Send end string		length: 1
    						// Protocol total		length: 36
  }
  else { // create a json send command string
  	message = Newtonsoft.Json.JsonConvert.SerializeObject(dyn);
  }
  
  if (!IsDemo && IsSerialConnected)
    {    
    	SerialPort.SendMessage(message);
    	Log(message);
  	}
  else
    {
    	Log(message + " (DEMO)");
  	}   
  return true;
};
```
### Supported api calls
| API call             	| description                                                                 	|
|-----------------------|-------------------------------------------------------------------------------|
| Control.On  	       	| example = ipserver + /api/Rfm/5/Control.On					|
| Control.Off	       	| example = ...									|
| Control.Enable	|										|
| Control.Disable	|										|
| Control.Status	| get level, duration and enable status						|
| Control.Parameter 	| get name, board, port, duration, on/off icons and enabled status		|
| Controls.Enable	| set ALL wizard driven module to enabled					|
| Controls.Disable	| set ALL wizard driven module to disabled and switch them off			|
| Controls.Stop		| switch off all 'still active' moduls						|
| Program.GetIcons	| get a list of all stored icons in path ...					|
| Program.Parameter	| get domain, serial port, baud rate, interval ... from program			|
| Program.GetBoardPins	| return a list of all possible board pins					|
| Rfm.IconOn		| set the module wizard 'on icon image'						|
| Rfm.IconOff		| set the module wizard 'off icon image'					|
| Rfm.Boardtype		| set the dedicated board type of a module					|
| Rfm.Duration		| set the switch on duration							|
| Rfm.Node		| set the module node								|
| Rfm.Port		| set the module port								|
| On			| switch on a free selected node without a existing virtual module		|
| Off			| switch on a free selected node without a existing virtual module		|

## Enjoy this package as a helper to develop your own things
**Wolfgang Otto, September 2016**
