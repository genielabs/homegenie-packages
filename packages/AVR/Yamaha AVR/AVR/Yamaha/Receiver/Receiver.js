[{
    Name: "Yamaha AVR",
    Author: "Dan Santee",
    Version: "2/16/2015",

    GroupName: "",
    IconImage: "pages/control/widgets/AVR/Yamaha/Receiver.png",
    StatusText: "",
    Description: "",
    UpdateTime: "",

    RenderView: function (cuid, module) {
        var widget = $(cuid).find("[data-ui-field=widget]");

        var last_updated = HG.WebApp.Utility.GetModulePropertyByName(module, "Status.LastUpdate").Value;
        widget.find("[data-ui-field=last_updated_value]").html(last_updated);

        var statusPower = HG.WebApp.Utility.GetModulePropertyByName(module, "Status.Power").Value;
        widget.find("#status_power").val(statusPower).flipswitch("refresh");

        var statusDsp = HG.WebApp.Utility.GetModulePropertyByName(module, "Status.DSP").Value;
        widget.find("#status_dsp").val(statusDsp).selectmenu("refresh");

        var statusInput = HG.WebApp.Utility.GetModulePropertyByName(module, "Status.Input").Value;
        widget.find("#status_input").val(statusInput.trim()).selectmenu("refresh");

        var statusVolume = HG.WebApp.Utility.GetModulePropertyByName(module, "Status.Volume").Value;
        widget.find("[data-ui-field=status_volume]").html(statusVolume);

        var widgeticon = HG.WebApp.Utility.GetModulePropertyByName(module, "Widget.DisplayIcon");
        if (widgeticon != null && widgeticon.Value != "") {
            this.IconImage = widgeticon.Value;
        }
        widget.find("[data-ui-field=icon]").attr("src", this.IconImage);

        widget.find("[data-ui-field=name]").html(module.Name);

        if (!this.Initialized) {
            this.Initialized = true;
            widget.find("#status_power").change(function () {
                HG.Control.Modules.ServiceCall($(this).val() == "On" ? "Control.On" : "Control.Off", module.Domain, module.Address, " ", function (data) { });
            });
            widget.find("#volume_up").click(function () {
                HG.Control.Modules.ServiceCall("Control.Volume", module.Domain, module.Address, "Up", function (data) { });
            });
            widget.find("#volume_down").click(function () {
                HG.Control.Modules.ServiceCall("Control.Volume", module.Domain, module.Address, "Down", function (data) { });
            });
            widget.find("#status_input").change(function () {
                HG.Control.Modules.ServiceCall("Control.Input", module.Domain, module.Address, $(this).val().replace(" ", "%20"), function (data) { });
            });
            widget.find("#status_dsp").change(function () {
                HG.Control.Modules.ServiceCall("Control.DSP", module.Domain, module.Address, $(this).val().replace(" ", "%20"), function (data) { });
            });
            widget.find("[data-ui-field=settings]").on("click", function () {
                HG.WebApp.ProgramEdit._CurrentProgram.Domain = module.Domain;
                HG.WebApp.ProgramEdit._CurrentProgram.Address = module.Address;
                HG.WebApp.ProgramsList.UpdateOptionsPopup();
            });
        }
    }

}]