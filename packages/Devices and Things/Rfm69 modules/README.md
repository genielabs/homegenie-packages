	# Arduino clone with Rfm69 transceiver system

Use Arduino clone Moteino or MoteinoMEGA and others (could be added easily) to control remote Rfm69 driven switches.

This app is written as a full functional EDUCATION version to help others understanding the full functionality of HomeGenie and the new widget style V2.

The app is devided into the HGX file with a C# program to act as the RESTFull service for all requests from assigned widget AND a V2 JavaScript widget with extended functionalities like 
- free configuration of Rfm69 nodes, pins and address 
- separate log file for all interactions
- Graph to display all switch times with filterable content
- icon file upload functionality
- selector for on/off icons

## Instruction

- Install the package with app and widget.
  The package includes also some imgaes and language files for English and German (others could be easily added)

- Start hgx programm called 'Rfm Switch Program'
- this package is 'preconfigured' and uses the foloowing parameters
	Rfm.NetId: 			7  		- Network ID for all remote devices
	Rfm.Port:  			/dev/ttyUSB0 	- Serial port of HomeGenie server to connect the Arduino clone board which acts as the central tranceiver
	Rfm.Baudrate:			19200		- Serial baud rate (19200)
	Rfm.Interval:			5		- Interval to send commands to clone
	Rfm.UseQueue:			True		- Use command queue to collect command and send them in bulk every interval
	Rfm.QueueSize:			5		- How many commands should be queued before sending
  	Rfm.VirtualModulesCount:	5		- Number of virtual modules
	Rfm.UseLog:			On		- Use debug logging
	Rfm.LogPath:			/usr/local/...	- Path of log file

- Configure a prefered group and select a 'Rfm switch' module.

- The module widget supports the following module parameters:
	Node:				address of RFM69 module to send commands to
	Boardtype:			Moteino or MoteinoMEGA, could be easily modified in HGX app
	Pin:				Available pins of selected board, could be easily modified in HGX app
	On Icon:			Select an ON icon out of server side image path
	Off Icon:			Select an OFF icon out of server side image path

	Support for uploading PNG images to server path
	
and you can directly select a duration time and switch device on or off
	- switch Duration (1 min, 5 min, 10 min, 15 min, 30 min, 45 min, 60 min predefined) 
	- ON / Off slider

### Hardware needs

- Any serial connected microcontroller driven device with an Rfm69 transceiver chip as the Gateway device (i used Moteino and MoteinoMEGA from https://lowpowerlab.com/
- multiple Rfm69 transceiver driven devices as actors or sensor (sensor part i not included in the example)

See <a href="https://lowpowerlab.com/category/moteino/">detailed instructions of Moteino here</a>.

