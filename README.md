# Homegenie Packages

Packages repository of user-contributed Automation Programs, Widgets and MIG Interfaces

## Contributing

To submit a new package to the repository:

- create fork of <a href="https://github.com/genielabs/homegenie-packages" target="_blank">homegenie-packages</a>
- push your package files into a convenient packages category folder of your fork
- verify that the package get listed in HomeGenie by changing the repository URL in the package browser with your fork's URL
- verify that the package get installed correctly
- create a pull request to get your package listed in the official repository

## How to bundle an HomeGenie package

A package can consist of a folder containing one or more Automation Program, Widget and MIG Interface.
In the package folder also a **README.md** file and a **package.json** file must be included.

The **README.md** is a *markdown* formatted file containing a detailed description with usage instructions for the package.

The **package.json** file is a *JSON* formatted file containing the list of files that will be installed.

### packages.json file format

The following is an example **package.json** file:

    {
        "author": "Daniel Maillard (DaniMail)",
        "version": "v1.0",
        "title": "Fibaro RGBW",
        "description": "Adds ZWave Fibaro RGBWM-441 control capability to HomeGenie",
        "published": "2014-02-06T00:00:00Z",
        "sourcecode": "",
        "homepage": "http://www.homegenie.it/forum/index.php?topic=15.0", 
        "widgets": [
            { 
                "file": "DaniMail_fibaro_rgbw.zip",
                "name":"Fibaro RGBW",
                "description": "Widget for controlling Fibaro RGBWM-441"
            }
        ],
        "programs": [
            { 
                "file": "Fibaro_RGBW.hgx",
                "name":"Fibaro RGBW",
                "description": "Fibaro RGBW driver app",
                "uid": "100201",
                "group": "Z-Wave" 
            }
        ],
        "interfaces": [ ]
    }

The *widgets* array contains a list of *Widgets*' **.zip** archives. The **.zip** archive can be downloaded from the Widget Editor page of each widget.

The *programs* array contains a list of *Automation Programs* **.hgx** files. The **.hgx** file can be downladed from the Program Editor page of each program.

The *interfaces* array contains a list of *MIG Interfaces* **.zip** archive. See the <a href="https://github.com/genielabs/mig-interface-skelton" target="_blank">mig-interface-skelton</a> project page for more informations about how to bundle an interface **.zip** archive.


