/*
 *     Author: Gaël Le Pennec 
 *     Project Homepage: http://homegenie.it
 */

using System;
using System.Collections.Generic;
using System.Threading;
using MIG.Config;
using MIG.Interfaces.HomeAutomation.Commons;
using RFLink;

namespace MIG.Interfaces.HomeAutomation
{

    public class RFLink : MigInterface
    {

        #region Constantes
        private const string paramSrc = "RFLink Controller";
        private const string dataSrc = "RFLink Controller";
        private const string sourceId = "1";
        private const string descriptionModule = "";
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MIG.Interfaces.HomeAutomation.RFLink"/> class.
        /// </summary>
        public RFLink ()
        {
            //Module list intialize
            modules = new List<InterfaceModule> ();

             //Intialize Gateway
            rflink = new RFLinkLib ();
            rflink.EvtOnItemCreated += rflinkEvtOnItemCreated;
            rflink.EvtOnItemDeleted += rflinkEvtOnItemDeleted;
            rflink.EvtOnDatachanged += rflinkEvtOnDatachanged;
            rflink.EvtOnPropertychanged += rflinkEvtOnPropertychanged;
            rflink.EvtOnVersionchanged +=rflinkEvtOnVersionchanged;
            rflink.EvtLog += rflinkEvtLog;

            //Intialize convertion Array
            listDataFieldToPath = new string [(int)DataField.RGBW + 1];
                listDataFieldToPath [(int)DataField.SWITCH] = "Status.Switch";
                listDataFieldToPath [(int)DataField.CMD] = "Status.Command";
                listDataFieldToPath [(int)DataField.SET_LEVEL] = ModuleEvents.Status_Level;
                listDataFieldToPath [(int)DataField.TEMP] = ModuleEvents.Sensor_Temperature;
                listDataFieldToPath [(int)DataField.HUM] = ModuleEvents.Sensor_Humidity;
                listDataFieldToPath [(int)DataField.BARO] = "Sensor.Presure";
                listDataFieldToPath [(int)DataField.HSTATUS] = "Status.TempPrevision";
                listDataFieldToPath [(int)DataField.BFORECAST] = "Status.MeteoPrevision";
                listDataFieldToPath [(int)DataField.UV] = ModuleEvents.Sensor_Ultraviolet;
                listDataFieldToPath [(int)DataField.LUX] = ModuleEvents.Sensor_Luminance;
                listDataFieldToPath [(int)DataField.BAT] = ModuleEvents.Status_Battery;
                listDataFieldToPath [(int)DataField.RAIN] = "Sensor.Rain";
                listDataFieldToPath [(int)DataField.RAINRATE] = "Sensor.RainRate";
                listDataFieldToPath [(int)DataField.RAINTOT] = "Sensor.RainTotal";
                listDataFieldToPath [(int)DataField.WINSP] = "Sensor.WindSpeed";
                listDataFieldToPath [(int)DataField.AWINSP] = "Sensor.WindAverageSpeed";
                listDataFieldToPath [(int)DataField.WINGS] = "Sensor.WindSpeed";
                listDataFieldToPath [(int)DataField.WINDIR] = "Sensor.WindDirection";
                listDataFieldToPath [(int)DataField.WINCHL] = "Sensor.WindChill";
                listDataFieldToPath [(int)DataField.WINTMP] = "Sensor.WindTemp";
                listDataFieldToPath [(int)DataField.CHIME] = "Status.ChimeLevel";
                listDataFieldToPath [(int)DataField.SMOKEALERT] = "Sensor.SmokeAlert";
                listDataFieldToPath [(int)DataField.PIR] = "Sensor.PIR";
                listDataFieldToPath [(int)DataField.CO2] = "Sensor.CO2";
                listDataFieldToPath [(int)DataField.SOUND] = "Status.SoundLevels";
                listDataFieldToPath [(int)DataField.KWATT] = ModuleEvents.Meter_KwHour;
                listDataFieldToPath [(int)DataField.WATT] = ModuleEvents.Meter_Watts;
                listDataFieldToPath [(int)DataField.CURRENT] = ModuleEvents.Meter_AcCurrent;
                listDataFieldToPath [(int)DataField.CURRENT2] = ModuleEvents.Meter_AcCurrent + "2";
                listDataFieldToPath [(int)DataField.CURRENT3] = ModuleEvents.Meter_AcCurrent + "3";
                listDataFieldToPath [(int)DataField.DIST] = ModuleEvents.Sensor_Generic;
                listDataFieldToPath [(int)DataField.METER] =ModuleEvents.Sensor_Generic;
                listDataFieldToPath [(int)DataField.VOLT] = ModuleEvents.Meter_AcVoltage;
                listDataFieldToPath [(int)DataField.RGBW] = "Status.RGBW";



            //Intialize conversion parameter Array
            listParamFieldToPath = new string [(int)ParamField.CC2500_Mode + 1];
                listParamFieldToPath [(int)ParamField.rfdebug] = "Controller.RFDEBUG";
                listParamFieldToPath [(int)ParamField.rfudebug] = "Controller.RFUDEBUG";
                listParamFieldToPath [(int)ParamField.qrfdebug] = "Controller.QRFDEBUG";
                listParamFieldToPath [(int)ParamField.setRF433] = "Controller.RF433";
                listParamFieldToPath [(int)ParamField.setNodoNRF] = "Controller.NodoNRF";
                listParamFieldToPath [(int)ParamField.setMilight] = "Controller.Milight";
                listParamFieldToPath [(int)ParamField.setLivingColors] = "Controller.LivingColors";
                listParamFieldToPath [(int)ParamField.setAnsluta] = "Controller.Ansluta";
                listParamFieldToPath [(int)ParamField.setGPIO] = "Controller.GPIO";
                listParamFieldToPath [(int)ParamField.setBLE] = "Controller.BLE";
                listParamFieldToPath [(int)ParamField.setMysensors] = "Controller.Mysensors";
                listParamFieldToPath [(int)ParamField.NRF24L01_Mode] = "Controller.NRF24L01";
                listParamFieldToPath [(int)ParamField.CC2500_Mode] = "Controller.CC2500";




            //Initilize all commands
            listCommand = new List<CommandExecuter> (
            new CommandExecuter []{
                new CommandExecuter ("Controller.Reboot",( receiveddata) =>{
                    rflink.RebootGateway ();
                    return "";
                } ),
                    new CommandExecuter ("Controller.DEBUG",(receiveddata) =>{
                    rflink.Debug= (bool)bool.Parse(receiveddata.GetOption(0));
                    return "";
                } ),
                    new CommandExecuter ("Controller.RFDEBUG",(receiveddata) =>{
                    rflink.RFDebug= (bool)bool.Parse(receiveddata.GetOption(0));
                    return "";
                } ),
                new CommandExecuter ("Controller.RFUDEBUG",(receiveddata) =>{
                    rflink.RFuDebug= (bool)bool.Parse(receiveddata.GetOption(0));
                    return "";
                } ),
                new CommandExecuter ("Controller.QRFDEBUG",(receiveddata) =>{
                    rflink.QRFDebug= (bool)bool.Parse(receiveddata.GetOption(0));
                    return "";
                } ),
                new CommandExecuter ("Controller.RF433",(receiveddata) =>{
                    rflink.RF433= (bool)bool.Parse(receiveddata.GetOption(0));
                    return "";
                } ),
                new CommandExecuter ("Controller.NodoNRF",(receiveddata) =>{
                    rflink.NodoNRF= (bool)bool.Parse(receiveddata.GetOption(0));
                    return "";
                } ),
                new CommandExecuter ("Controller.NRF24L01",(receiveddata) =>{
                        int val =int.Parse (receiveddata?.GetOption (0));
                        NRF24L01Mode b = (NRF24L01Mode) Enum.ToObject (typeof (NRF24L01Mode),val);
                        rflink.NRF24L01Mode=b;
                                    return "";
                } ),
                new CommandExecuter ("Controller.GPIO",(receiveddata) =>{
                    rflink.GPIO= (bool)bool.Parse(receiveddata.GetOption(0));
                    return "";
                } ),
                new CommandExecuter ("Controller.CC2500",(receiveddata) =>{
                        int val =int.Parse (receiveddata?.GetOption (0));
                        CC2500Mode b = (CC2500Mode) Enum.ToObject (typeof (CC2500Mode),val);
                        rflink.CC2500Mode= b;
                                    return "";
                } ),
                new CommandExecuter ("Controller.TimeoutForDropMember",(receiveddata) =>{
                    var ret = (byte)int.Parse(receiveddata.GetOption(0));
                    rflink.TimeoutForDropMember =ret;
   
                    return "";
                } ),
                 new CommandExecuter ("Controller.Version",(receiveddata) =>{
                    try {
                            InterfacePropertyChanged?.Invoke  (this,new InterfacePropertyChangedEventArgs
                                                               (this.GetDomain ()
                                                                , sourceId
                                                                , dataSrc
                                                                , "Controller.Version"
                                                                , rflink?.Version));

                        rflinkEvtOnPropertychanged(ParamField.NRF24L01_Mode,(int)rflink.NRF24L01Mode);
                        rflinkEvtOnPropertychanged(ParamField.CC2500_Mode,(int)rflink.CC2500Mode);

                    } catch (Exception ex) {
                            var error = ex.Message;
                    }
                    return "";
                } ),
               new CommandExecuter ("Controller.Status",(receiveddata) =>{
                     rflink.StatusGateway ();
                    return "";
                } ),
                new CommandExecuter ("Controller.Logs",(receiveddata) =>{
                    lastLogReceived = DateTime.Now;
                        if(logWatcher == null){
                            logWatcher = new Timer ((state) => {
                                if (DateTime.Compare (lastLogReceived.AddSeconds (10), DateTime.Now) < 0){
                                       try
                                        {
                                            logWatcher.Dispose();
                                            logWatcher = null;
                                        }
                                        catch {

                                        }
                                }
                            },null,0,5000);
                        }
                    return "";
                } ),
                new CommandExecuter ("Control.Remote",(receiveddata) =>{
                            try{

                                var protocol = receiveddata.GetOption(0);
                                var id = receiveddata.GetOption(1);
                                var chanal =receiveddata.GetOption(2) ;
                                var cmd =receiveddata.GetOption(3) ;
                                var ack =receiveddata.GetOption(4) ;

                                rflink.SendRemoteCommand (protocol,id,chanal,cmd);

                            if (ack == null || ack == "" || bool.Parse (ack)){
                                //return acknowledgment
                          
                                InterfacePropertyChanged?.Invoke  (this,new InterfacePropertyChangedEventArgs
                                   (this.GetDomain ()
                                    , id
                                    , dataSrc
                                    , "Status.Result"
                                    , "SENDED"));
                            }
                                 return "";
                            }catch(Exception ex){
                                return  ex.Message;
                            }

                } )

            }
        );

        }

        #endregion

        #region private Fields

        private RFLinkLib rflink = null;

        private List<InterfaceModule> modules;

        private static string [] listDataFieldToPath;
        private static string [] listParamFieldToPath;

        private Timer logWatcher;
        private DateTime lastLogReceived;

        #endregion

        #region Events
        void rflinkEvtOnItemCreated (string id)
        {
            modules.Add (new InterfaceModule () { Domain = this.GetDomain (), Address = id, ModuleType = ModuleTypes.Generic ,Description=descriptionModule });
            InterfaceModulesChanged?.Invoke (this,new InterfaceModulesChangedEventArgs (this.GetDomain ()));
        }

        void rflinkEvtOnItemDeleted (string id)
        {
            var search = modules.Find (x => x.Address == id);
            if (search != null) {
                modules.Remove (search);
                InterfaceModulesChanged?.Invoke (this, new InterfaceModulesChangedEventArgs (this.GetDomain ()));
            }
        }

        /// <summary>
        /// Rflinks the evt on datachanged.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="type">Type.</param>
        /// <param name="data">Data.</param>
        void rflinkEvtOnDatachanged (string id, DataField type, object data)
        {
            try
             {
                InterfacePropertyChanged?.Invoke (this, 
                                                  new InterfacePropertyChangedEventArgs (
                                                      this.GetDomain ()
                                                      , id
                                                      , dataSrc
                                                      , listDataFieldToPath [(int)type]
                                                      , data));
                 
             } catch {  
            
            }
        }

        /// <summary>
        /// Rflinks the evt on propertychanged.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="data">Data.</param>
        void rflinkEvtOnPropertychanged (ParamField type, object data)
        {
            try {
                InterfacePropertyChanged?.Invoke (this,
                                                  new InterfacePropertyChangedEventArgs (
                                                      this.GetDomain ()
                                                      , sourceId
                                                      , dataSrc
                                                      , listParamFieldToPath [(int)type]
                                                      , data));

                //Options are updated
                InterfaceModulesChanged?.Invoke (this, new InterfaceModulesChangedEventArgs (this.GetDomain ()));
            } catch { 
                
            }
        }

        void rflinkEvtOnVersionchanged ()
        {
            try {
                InterfacePropertyChanged?.Invoke (this,
                                                  new InterfacePropertyChangedEventArgs (
                                                      this.GetDomain ()
                                                      , sourceId
                                                      , dataSrc
                                                      , "Controller.Version"
                                                      , rflink?.Version));

            } catch { 
                
            }
        }

        void rflinkEvtLog (string id)
        {
            try {
                if(logWatcher != null){
                    InterfacePropertyChanged?.Invoke (this,
                                                  new InterfacePropertyChangedEventArgs (
                                                      this.GetDomain ()
                                                      , sourceId
                                                      , dataSrc
                                                      , "Controller.Logs"
                                                      , id));

                }
               
            } catch {

            }
        }
        #endregion

        #region Implemented MIG Commands
        private List<CommandExecuter> listCommand;

        public class CommandExecuter{
            public delegate string CommandExecuterCallBack(MigInterfaceCommand receiveddata);
            private CommandExecuterCallBack callBack;
            public string Id { get; private set; }

            public CommandExecuter(string id,CommandExecuterCallBack callback ){
                Id = id;
                callBack = callback;
            }

            public string CommandTraitment(MigInterfaceCommand receiveddata){
                return callBack?.Invoke (receiveddata);
            }
        }
        #endregion

        #region MIG Interface members
        public event InterfaceModulesChangedEventHandler InterfaceModulesChanged;
        public event InterfacePropertyChangedEventHandler InterfacePropertyChanged;

        public bool IsEnabled { get; set; }

        public List<Option> Options { get; set; }

        public void OnSetOption (Option option){
            //TODO Reconnect or not and replace option 
            if(IsEnabled){
                switch (option.Name) {
                case "Port":
                    Disconnect ();
                    Connect ();
                    break;
                case "RF433":
                    rflink.RF433 = (bool)bool.Parse (option.Value);
                    break;
                case "NodoNRF":
                    rflink.NodoNRF = (bool)bool.Parse (option.Value);
                    break;
                case "GPIO":
                    rflink.GPIO = (bool)bool.Parse (option.Value);
                    break;
                case "NRF24L01_Mode":
                    rflink.NRF24L01Mode = (NRF24L01Mode)Enum.ToObject (typeof (NRF24L01Mode), int.Parse (option.Value));
                    break;
                case "CC2500_Mode":
                    rflink.CC2500Mode = (CC2500Mode)Enum.ToObject (typeof (CC2500Mode), int.Parse (option.Value)); 
                    break;
                default:
                    break;
                }
            }
        }


        public List<InterfaceModule> GetModules ()
        {
            return modules;
        }

        /// <summary>
        /// Connect to the automation interface/controller device.
        /// </summary>
        public bool Connect ()
        {
            InterfaceModulesChanged?.Invoke (this,new InterfaceModulesChangedEventArgs (this.GetDomain ()));

            syncOptions ();
            rflink.Connect ();

            return true;
        }
        /// <summary>
        /// Disconnect the automation interface/controller device.
        /// </summary>
        public void Disconnect ()
        {
            rflink.Disconnect ();
        }
        /// <summary>
        /// Gets a value indicating whether the interface/controller device is connected or not.
        /// </summary>
        /// <value>
        /// <c>true</c> if it is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected {
            get { return rflink?.IsConnected ?? false; }
        }
        /// <summary>
        /// Returns true if the device has been found in the system
        /// </summary>
        /// <returns></returns>
        public bool IsDevicePresent ()
        {
            // eg. check against libusb for device presence by vendorId and productId
            return true;
        }


        public object InterfaceControl (MigInterfaceCommand request)
        {
            try{
                var search = listCommand.Find (x => x.Id == request.Command);
                if (search != null) {
                    return search.CommandTraitment (request);
                }

                return "Command Not found in this gateway";
            }catch(Exception ex){
                return ex.Message;
            }
        }
        #endregion

        #region private methods
        private string default_Option (Enum name, string data)
        {
            return default_Option (name.ToString (), data);
        }

        private string default_Option (string name, string data, bool withrestart = false)
        {
            var ret = this.GetOption (name);
            if (ret == null) {

                 this.SetOption (name, data);

                return data;
            }
            return ret.Value;
        }

        private void syncOptions(){
            
            rflink.PortName =  default_Option ("Port", null);

            rflink.RFDebug = bool.Parse (default_Option (ParamField.rfdebug, false.ToString ()));
            rflink.RFuDebug = bool.Parse (default_Option (ParamField.rfudebug, false.ToString ()));
            rflink.QRFDebug = bool.Parse (default_Option (ParamField.qrfdebug, false.ToString ()));

            rflink.RF433 = bool.Parse (default_Option (ParamField.setRF433, true.ToString ()));
            rflink.NodoNRF = bool.Parse (default_Option (ParamField.setNodoNRF, false.ToString ()));

            rflink.GPIO = bool.Parse (default_Option (ParamField.setGPIO, false.ToString ()));

            rflink.NRF24L01Mode =(NRF24L01Mode) Enum.Parse (typeof (NRF24L01Mode), default_Option (ParamField.NRF24L01_Mode, ((int)NRF24L01Mode.Disable).ToString ()));
            rflink.CC2500Mode = (CC2500Mode)Enum.Parse (typeof (CC2500Mode), default_Option (ParamField.CC2500_Mode, ((int)CC2500Mode.Disable).ToString ()));

        }

        #endregion

    }

}
