# MIG-Harmony interface
HomeGenie MIG interface driver for Logitech Harmony. Tested on HarmonyHub on Raspberry Pi 2 and Windows 10.
Using https://github.com/hdurdle/harmony for the communication with harmony. 

##Installation
Install in HomeGenie, use the package "mig_interface_harmony_zip"

The interface has 3 settings that are required (set these before enableing the interface:
* Username (required): Username to the Logitech's web service. ex username@somemail.com
* Password (required): Password to the Logitech's web service
* IPAddress (required): Local ip-address to harmony hub
* Experimental: 1-2

Goto configure modules and add the desired activities

##Features
Currently the interface supports the following:
* Reading all harmony activities and adding them to HomeGenie
* Starting and stopping activities

##Next steps
* Adding the settings to the GUI, this should be straight forward
* Improving Session handling
* Add support for remote control buttons (again, should be an easy task)
* Custom widget
* Refactoring and bugfixes, not very clean at the moment  ::) 
 
# I will gladly accept pull requests!