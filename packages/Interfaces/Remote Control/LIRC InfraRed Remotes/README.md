# LIRC InfraRed I/O control

Send and receive IR signals. This interface driver is based on LIRC package.

## Instructions

LIRC package must be installed and configured for this driver to work.

### 1. Install and configure LIRC

    sudo apt-get install lirc liblircclient-dev

To enable InfraRed remote control edit the /etc/lirc/hardware.conf file:

   sudo nano /etc/lirc/hardware.conf

Paste the following text into it: 

    ########################################################
    # /etc/lirc/hardware.conf
    #
    # Arguments which will be used when launching lircd
    LIRCD_ARGS="--uinput"
    
    # Don't start lircmd even if there seems to be a good config file
    # START_LIRCMD=false
    
    # Don't start irexec, even if a good config file seems to exist.
    # START_IREXEC=false
    
    # Try to load appropriate kernel modules
    LOAD_MODULES=true
    
    # Run "lircd --driver=help" for a list of supported drivers.
    DRIVER="default"
    # usually /dev/lirc0 is the correct setting for systems using udev
    DEVICE="/dev/lirc0"
    MODULES="mceusb"
    
    # Default configuration files for your hardware if any
    LIRCD_CONF=""
    LIRCMD_CONF=""
    ########################################################

Change **MODULES="mceusb"** line with your IR transceiver module name. 

#### 1.1 Raspberry Pi GPIO IR

If you are using Raspberry Pi GPIO IR hardware, enable the **lirc_rpi** kernel module, then change the above mentioned line to **MODULES="lirc_rpi"**. 
For more information about IR GPIO module for Raspberry Pi see <a href="http://aron.ws/projects/lirc_rpi/" target="_blank">Raspberry Pi lirc_rpi</a>.

#### 1.2 CubieBoard and Banana Pi built-in IR receiver

If you want to use built-in *Banana Pi / CubieBoard* IR receiver, follow these <a href="http://linux-sunxi.org/LIRC#Using_LIRC_with_Cubieboard2_.28A20_SoC.29" target="_blank">instructions</a> instead.

### 2. Restart LIRC

   sudo /etc/init.d/lirc restart

Happy remote controlling! =) 


