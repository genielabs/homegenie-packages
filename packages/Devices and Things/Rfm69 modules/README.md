# System to control Arduino clones with a transceiver system (EDUCATION version)
---
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
---
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
---
Any serial connected microcontroller driven device with an Rfm69 transceiver chip as the Gateway device (i used Moteino and MoteinoMEGA from https://lowpowerlab.com/
multiple Rfm69 transceiver driven devices as actors or sensor (sensor part i not included in the example)

See <a href="https://lowpowerlab.com/category/moteino/">detailed instructions of Moteino here</a>.   

**Wolfgang Otto, September 2016**

