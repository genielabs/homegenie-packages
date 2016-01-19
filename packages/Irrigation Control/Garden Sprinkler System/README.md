# Garden Sprinkler System

Activate sprinkler system (irrigation system) based on CRON scheduler.
The app provides the logic to run the sprinkler system on a Cron based time schedule, the number of groups to activate and the sprinkler time.
This needs to run on a Raspberry Pi, the app is configured for the GPIO pins of the Raspberry Pi B+, but with a little change of the code it can be made to work on the Raspberry Pi A or B.
This is also compatible with the <a href="http://www.piface.org.uk/products/piface_digital/" target="_blank">PiFace Digital</a> board.

## Instructions

The second part of the Garden Sprinkler System, the relay module for driving the sprinkler solenoids, requires some handy work and soldering.

### Hardware needs

- 1 x LED
- 1 x 220 or 330K Resistor
- 6 x 1K resistor
- 6 x NPN transistor 2N3904
- 1 x <a href="http://www.amazon.com/gp/product/B00N1X5CM4/ref=ox_sc_sfl_title_2?ie=UTF8&psc=1&smid=A325CF4XAXVINN" target="_blank">Prototype board</a>
- 1 x <a href="http://www.amazon.com/gp/product/B00C8O9KHA/ref=oh_aui_detailpage_o06_s01?ie=UTF8&psc=1" target="_blank">8 Relay Module Board</a>
- Some wire and connectors

See <a href="http://www.homegenie.it/forum/index.php?topic=594.0">detailed instructions here</a>.

## Usage

The Garden Sprinkler System App has 3 individual schedules, Schedule A, B and C.
For each schedule you can program a time and date based on CRON and assign the individual sprinkler groups.
This should give full flexibility for even the most demanding and creative schedule you may want to use.

**Make sure that Schedule A, B and C do not overlap in time!**

*CRON:*

    * * * * * ( 1 2 3 4 5 ) command to execute
    1 = min (0 to 59)
    2 = hour (0 to 23)
    3 = day of month (1 to 31) (1-31/2 = odd days, 2-30/2 = even days)
    4 = month (1 to 12)
    5 = day of week (0 to 6) (0 to 6 are Sunday to Saturday

*Groups:*

    1,2,3,4,5,6 = all groups active
    1,3,6 = group 1, 3 and 6 active
    none = no group active


