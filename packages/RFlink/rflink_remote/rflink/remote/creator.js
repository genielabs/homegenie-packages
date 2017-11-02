[{
    Name: "Generic Program",
    Author: "Generoso Martello",
    Version: "2013-03-31",

    GroupName: '',
    IconImage: 'pages/control/widgets/rflink/remote/images/fireon.png',
    StatusText: '',
    Description: '',
    Initialized: false,

    RenderView: function (cuid, module) {
        var container = $(cuid);
        var widget = container.find('[data-ui-field=widget]');
        var btn = container.find('[data-ui-field=btn]');
        var label = container.find('[data-ui-field=name]');
        //
        // read some context data
        //
        this.GroupName = container.attr('data-context-group');
        //
        if (!this.Initialized) {
            this.Initialized = true;
            
            var groupname = this.GroupName;
            widget.on('click', function () {
                HG.Automation.Programs.Toggle(module.Address, groupname, null);
            });
            // settings button
            widget.find('[data-ui-field=settings]').on('click', function () {
                HG.WebApp.ProgramEdit._CurrentProgram.Domain = module.Domain;
                HG.WebApp.ProgramEdit._CurrentProgram.Address = module.Address;
                HG.WebApp.ProgramsList.UpdateOptionsPopup();
            });
        }
        //
        var prog = HG.WebApp.ProgramsList.GetProgramByAddress(module.Address);
        var statuscolor = 'black';
        var status = HG.WebApp.Utility.GetModulePropertyByName(module, "Program.Status");
        if (status != null) {
            switch (status.Value) {
                case 'Enabled':
                case 'Idle':
                    statuscolor = 'yellow';
                    break;
                case 'Running':
                    statuscolor = 'green';
                    break;
                case 'Disabled':
                    statuscolor = 'black';
                    if (prog.Type.toLowerCase() != 'wizard' && prog.ScriptErrors.trim() != '' && prog.ScriptErrors.trim() != '[]') {
                        if (prog.IsEnabled) {
                            statuscolor = 'red';
                        }
                        else {
                            statuscolor = 'brown';
                        }
                    }
                    break;
            }
        }
        //
        // render widget
        //
        label.html(HG.WebApp.Locales.GetProgramLocaleString(module.Address, 'Title', module.Name.substring(module.Name.lastIndexOf('|') + 1)));
        btn.css('background-image', 'url(images/common/led_' + statuscolor + '.png)');
    }
}]
