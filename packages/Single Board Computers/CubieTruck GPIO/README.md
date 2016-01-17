# GPIO Modules

CubieTruck/CubieBoard3 GPIOs mapped to HG modules.

Each GPIO can be configured as input (IN), output (OUT) or can be disabled (OFF).

GPIOs configured as IN are mapped to a Sensor module with a Status.Level field displaying current pin level (0, 1).

GPIOs configured as OUT are mapped to a Switch module that can be controlled with on/off commands.

## Enabling GPIO programming on CubieBoard

To enable GPIO programming with HomeGenie on CubieBoard, the **[gpio_para]** section of the **script.fex** file must be configured as shown below:

    [gpio_para]
    gpio_used = 1
    gpio_num = 32
    gpio_pin_1 = port:PH20<1><default><default><1>
    gpio_pin_2 = port:PH10<0><default><default><0>
    gpio_pin_3 = port:PC19<0><default><default><0>
    gpio_pin_4 = port:PC21<0><default><default><0>
    gpio_pin_5 = port:PC20<0><default><default><0>
    gpio_pin_6 = port:PC22<0><default><default><0>
    gpio_pin_7 = port:PB14<0><default><default><0>
    gpio_pin_8 = port:PB16<0><default><default><0>
    gpio_pin_9 = port:PB15<0><default><default><0>
    gpio_pin_10 = port:PB17<0><default><default><0>
    gpio_pin_11 = port:PI20<0><default><default><0>
    gpio_pin_12 = port:PI14<0><default><default><0>
    gpio_pin_13 = port:PI21<0><default><default><0>
    gpio_pin_14 = port:PI15<0><default><default><0>
    gpio_pin_15 = port:PI3<0><default><default><0>
    gpio_pin_16 = port:PB3<0><default><default><0>
    gpio_pin_17 = port:PB2<0><default><default><0>
    gpio_pin_18 = port:PB4<0><default><default><0>
    gpio_pin_19 = port:PB18<0><default><default><0>
    gpio_pin_20 = port:PB19<0><default><default><0>
    gpio_pin_21 = port:PG0<0><default><default><0>
    gpio_pin_22 = port:PG3<0><default><default><0>
    gpio_pin_23 = port:PG2<0><default><default><0>
    gpio_pin_24 = port:PG1<0><default><default><0>
    gpio_pin_25 = port:PG4<0><default><default><0>
    gpio_pin_26 = port:PG5<0><default><default><0>
    gpio_pin_27 = port:PG6<0><default><default><0>
    gpio_pin_28 = port:PG7<0><default><default><0>
    gpio_pin_29 = port:PG8<0><default><default><0>
    gpio_pin_30 = port:PG9<0><default><default><0>
    gpio_pin_31 = port:PG10<0><default><default><0>
    gpio_pin_32 = port:PG11<0><default><default><0>

For further instructions about how to change the system configuration on CubieBoar, see the following <a target="_blank" href="http://docs.cubieboard.org/tutorials/common/edit_the_system_configuration">tutorial</a>.
    
