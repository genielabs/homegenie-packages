# Global Cache IR

This automation program module support Global Cache IP2IR and WF2IR. Might work with Global Cache itach flex too if it is using the same TCP API. I only have IP2IR for now.

These devices can send signals to control any infra red devices. Currently I use it for tv, projector, amplifier, HDMI 4x 2x matrix switch, dreambox, android box and air-conditioner. A number of interesting stuff can be achieved by controlling infra-red devices :) Program has been used for a number of months and it works well.

Only Major problem I have encountered so far is when the module is accessed while the global cache device is not in the network, after which the global cache and/or HomeGenie devices might need rebooting. So I access the Global Cache device only when it is on the network. (Note: It takes a few seconds for the global cache device to be in the network after switching it's power on)

The module can support multiple global cache devices, but an extra line for every device is needed to be added. Comments in the Trigger code should help achieving this.  Keep in mind I did this program with nearly no experience on C and HomeGenie, so there could be possibility of improvements to make it better and easier to use. So, hopefully others will improve it and maybe also eventually added in HomeGenie.

I have added an example of a TV module, to show how it can be used. I have uploaded this since it is the easiest example I have.

**Once the module is installed, the Global Cache IP input field need to be inserted.**

Then any other module can access the Global Cache module like the following:

    var globalCacheModuleNumber = "1"; // Setting the Global Cache module number to use in case there is more than one Global Cache device.
    var globalCacheModules = Modules.InDomain("HomeAutomation.GlobalCache");
    var globalCacheModule = globalCacheModules.WithAddress(globalCacheModuleNumber).Get();

    string tvRemoteOn = "sendir,1:3,1,38343,1,1,170,172,21,65,21,65,21,65,21,22,21,22,21,22,............, 21,22"; // Change this to the IR command needed
    globalCacheModule.Command("Control.Send").Set(tvRemoteOn); // Will simply execute the required IR code on the Global Cache device


