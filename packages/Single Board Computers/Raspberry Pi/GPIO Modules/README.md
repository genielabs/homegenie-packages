# GPIO Modules

Raspberry Pi GPIO mapped to HG modules.

From the program options, each GPIO can be configured as input (IN), output (OUT) or can be disabled (OFF).

Use `IN+` to activate the internal PullUp resistor or `IN-` to activate the internal PullDown resistor.

Use `!OUT` or `!IN+` or `!IN-` to Reverse the pin level `On = 0` and `Off = 1`.

GPIOs configured as IN are mapped to a *Sensor module* with a `Status.Level` field displaying current pin level (0, 1) or inverted in case of `!IN`.

GPIOs configured as OUT are mapped to a *Switch module* that can be controlled with on/off commands.


## Videos

<a href="https://www.youtube.com/watch?v=bpZ5Y2UqBUc" target="_blank">HomeGenie + Raspberry Pi GPIO (Setup and Wizard Scriptng)</a>

<a href="https://www.youtube.com/watch?v=kGF3C3g4a7M" target="_blank">HomeGenie + Raspberry Pi GPIO (Javascript and Python example)</a>

<a href="https://www.youtube.com/watch?v=VEwVNfFiodQ" target="_blank">HomeGenie meets Banana Pi</a>


## Changes

### v1.1 - Fixes for !IN and Status.Level

- Fix reverted change for `!IN` commit [c95cd6e](https://github.com/genielabs/homegenie-packages/commit/c95cd6e359b7bde04cf1a49a8b878aa6d21f211d)
- Status.Level logic has been changes for `!IN` and `!OUT`, but is wrong as they only change the pin level not the logical level (i.e. `!OUT Status.Level = On` pin level` = 0`).
- Update deprecated api call from `Program.RaiseEvent()` to `module.RaiseEvent()`
