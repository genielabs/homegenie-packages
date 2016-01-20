# Alarm Panel

It's my experience that alarm sensors (door/window and motion) are more reliable and less expensive than their HA counterparts. Also, if the HA system fails, the alarm panel will still do its job as an alarm. With that in mind, I've been using my Honeywell Ademco Vista 20P alarm panel and wireless sensors as HA sensors in my previous HA setup, and wanted to do it again with HG.

The AD2USB connects to lots of different kinds of alarm panels, but my experience is only with the Vista 20P. It wires in as a keypad using the four-wire interface, and provides a USB connection to your computer. It creates a virtual serial port which can be used to get information from the panel, and to provide commands to the panel. I'm on Windows, but it should work fine on Mac and Linux, as well, as there are drivers available.

## Update 5/1/2015

You can now interact with the system using the plugin. Six quick-access buttons (Disarm, Max, Stay, Instant, Away and Chime) plus the 12-key keypad are available for use.

## Update 6/7/2015

The panel/sensor combo will now monitor both the panel and wireless sensors directly. You can add the RFX ID of your sensor to the Alarm Sensor module instead of a panel string to match. The RFX.Status field on the sensor is updated with loop/battery/supervision information.

The files provided will get you started. The AlarmPanel folder should be placed in /HomeGenie/html/pages/control/widgets, and then import the HGX files. The Alarm Panel program will provide the interface to the alarm panel itself, and the Alarm Sensor will provide the virtual sensors which feed off of the data from the Alarm Panel. There's a place to enter the string which the alarm panel sends when a zone is faulted - such as "FAULT 1 FRONT DOOR". These strings are set by the installer at the panel, and are what show up on the keypads when a zone is faulted. If you don't know all the strings, you can look at the Alarm Panel device that you create, and it will display them as they're faulted. Just copy and paste that text into the textbox. You can alternately use the RFX ID of the wireless sensor you want to monitor directly.
