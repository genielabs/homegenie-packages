# GPIO Modules

Raspberry Pi GPIO mapped to HG modules.

From the program options, each GPIO can be configured as input (IN), output (OUT) or can be disabled (OFF).

Use IN+ to activate the internal PullUp resistor or IN- to activate the internal PullDown resistor.

Use !OUT to Reverse the pin level On = 0 and Off = 1.

GPIOs configured as IN are mapped to a Sensor module with a Status.Level field displaying current pin level (0, 1).

GPIOs configured as OUT are mapped to a Switch module that can be controlled with on/off commands.

