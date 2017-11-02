[{
  Name: 'New Widget',
  Author: 'Foo Bar',
  Version: '1.0',
  GroupName: '',
  StatusText: '',
  Description: '',


  IconImage: 'pages/control/widgets/rflink/remote/images/fireon.png',
  Initialized: false,

  RenderView: function (cuid, module) {
    var container = $(cuid);
    var widget = container.find('[data-ui-field=widget]');
    var _this = this;
    if (!this.Initialized)
    {
      this.Initialized = true;
      // store a reference to ui fields
      this.nameText = widget.find('[data-ui-field=name]');
      this.descriptionText = widget.find('[data-ui-field=description]');
      this.statusText = widget.find('[data-ui-field=status]');
      this.iconImage = widget.find('[data-ui-field=icon]');
      this.nameButton_On = widget.find('[data-ui-field=on]');

      // handle ui elements events
      widget.find('[data-ui-field=on]').bind('click', function(){
        _this.OnClicked();
      });
      widget.find('[data-ui-field=off]').bind('click', function(){
        _this.OffClicked();
      });
      //Default property loader
      //For more details you can read RFLink manual http://www.rflink.nl/blog2/protref .
      this.protocolname = this.rflink_remotecrl_GetLocalProperty(module, "RFLink.ProtocolName", "Blyss"); // Protocol Name describe in manual.
      this.ModuleId =   this.rflink_remotecrl_GetLocalProperty(module, "RFLink.Id", "4A36"); // Channel Number in hexadecimal
      this.channel =   this.rflink_remotecrl_GetLocalProperty(module, "RFLink.Channel", "A1"); // Channel Number in hexadecimal
      this.commandname_on =  this.rflink_remotecrl_GetLocalProperty(module, "RFLink.On_CommandName", "ON"); //We can changed with ON/OFF/ALLON/ALLOFF/UP/DOWN/STOP/PAIR/ etc.
      this.commandname_off =  this.rflink_remotecrl_GetLocalProperty(module, "RFLink.Off_CommandName", "OFF"); //We can changed with ON/OFF/ALLON/ALLOFF/UP/DOWN/STOP/PAIR/ etc.
   
    }
    // render widget
    this.nameText.html(module.Name + " (" + module.DeviceType + ")");
    this.descriptionText.html('Hello World');
    this.nameButton_On.html(this.commandname_on);
  },

  OnClicked: function() {
    var _this = this;
    _this.iconImage.attr('src', 'pages/control/widgets/rflink/remote/images/fireon.png');
    _this.statusText.html(_this.commandname_on +' was sended!');
    HG.Control.Modules.ServiceCall("Control.Remote","HomeAutomation.RFLink","1",_this.protocolname + '/' + _this.ModuleId + '/' + _this.channel+ '/' + _this.commandname_on + '/true', function (data) { });
 	setTimeout(function() {_this.OffClicked()}, 1000);
  },

  OffClicked: function() {
    this.statusText.html('');
    this.iconImage.attr('src', 'pages/control/widgets/rflink/remote/images/fireoff.png');
  },
  
   rflink_remotecrl_GetLocalProperty:function (module, prop, defvalue) {
     var priProtocol = HG.WebApp.Utility.GetModulePropertyByName(module, prop);
     if (priProtocol == null ) {
        HG.WebApp.Utility.SetModulePropertyByName(module, prop,defvalue);
      	priProtocol = defvalue;
    }
     return priProtocol.Value;
 }
}]
