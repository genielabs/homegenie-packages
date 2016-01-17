# MIG-TelldusTellstick interface

HomeGenie / MIG interface driver for TellStick. Tested on TellStick Duo on Raspberry Pi 2 and 1 aswell as Windows 8.

**Before installing this package, follow the instructions below.**

## Features
Currently the interface supports the following:
- Turning lights on/off
- Dimming lights
- Reading temperature values
- Simulated two way communication (remote triggers homegenie status)

## Installation Instructions

### Install telldus-core

This interface requires telldus-core libraries. To install these in Windows, Mac or Linux follow instructions below.

#### Windows

Download telldus center from http://www.telldus.se/products/nativesoftware.

#### Raspberry pi

SSH into the pi and run the following commands:

Update apt-get:

    sudo nano /etc/apt/sources.list.d/telldus.list
    deb-src http://download.telldus.com/debian/ stable main
            
Download the key:

    wget http://download.telldus.se/debian/telldus-public.key 

Add the key:

    sudo apt-key add telldus-public.key

Run a update in order to add the telldus sources:

    sudo apt-get update

This should already be installed:

    sudo apt-get install build-essential

Install dependencies:

    sudo apt-get build-dep telldus-core

Even more dependencies:

    sudo apt-get install cmake libconfuse-dev libftdi-dev help2man
        
Make a temporary directory to compile in:

    mkdir -p ~/telldus-temp
    cd ~/telldus-temp
        
Download the source:

    sudo apt-get –-compile source telldus-core

Install: 

    sudo dpkg -–install *.deb
        
Done, you can now plug in your telldus usb device, if you have any sensors to test you can list the devices by typing:

    tdtool -l

The above steps are fetched from the swedish blogpost: <a href="https://blogg.itslav.nu/?p=875" target="_blank">https://blogg.itslav.nu/?p=875</a>
Here is another more official tutorial I haven't tried myself: http://elinux.org/R-Pi_Tellstick_core.

