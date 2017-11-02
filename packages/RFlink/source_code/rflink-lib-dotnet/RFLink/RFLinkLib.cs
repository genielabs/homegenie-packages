using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using SerialPortLib;
using static RFLink.RFLink_control;

namespace RFLink
{


    public class RFLinkLib
    {
        #region Constructor/Destructor
        public RFLinkLib ()
        {
            TimeoutForDropMember = 10;

            //Attach to static event log
            RFLink_control.EvtLog +=rflinkControlEvtLog;

            serialPort = new SerialPortInput ();
            //_serialPort.Debug = _debug;
            serialPort.ConnectionStatusChanged += serialPortConnectionStatusChanged;
            serialPort.MessageReceived +=serialPortMessageReceived;

            listProperties = new List<IFieldConverter<ParamField>> () {
                new FieldConverterBool<ParamField>(ParamField.rfdebug,handlePropertyChangedStandard),
                new FieldConverterBool<ParamField> (ParamField.rfudebug,handlePropertyChangedStandard),
                new FieldConverterBool<ParamField> (ParamField.qrfdebug,handlePropertyChangedStandard),
                new FieldConverterBool<ParamField>(ParamField.setRF433,handlePropertyChangedStandard){OtherName="RF433"},
                new FieldConverterBool<ParamField>(ParamField.setNodoNRF,handlePropertyChangedStandard){OtherName="NodoNRF"},
                new FieldConverterBool<ParamField>(ParamField.setMilight,(sender) =>{ handlePropertyChangedNRF24L01 (sender,NRF24L01Mode.MiLight);}){OtherName="Milight"},
                new FieldConverterBool<ParamField>(ParamField.setLivingColors,(sender) =>{ handlePropertyChangedCC2500 (sender,CC2500Mode.LivingColors);}){OtherName="LivingColors"},
                new FieldConverterBool<ParamField>(ParamField.setAnsluta,(sender) =>{ handlePropertyChangedCC2500 (sender,CC2500Mode.Ansluta);}){OtherName="Ansluta"},
                new FieldConverterBool<ParamField>(ParamField.setGPIO,handlePropertyChangedStandard){OtherName="GPIO"},
                new FieldConverterBool<ParamField>(ParamField.setBLE,(sender) =>{ handlePropertyChangedNRF24L01 (sender,NRF24L01Mode.BLE);}){OtherName="BLE"},
                new FieldConverterBool<ParamField>(ParamField.setMysensors,(sender) =>{ handlePropertyChangedNRF24L01 (sender,NRF24L01Mode.MySensors);}){OtherName="Mysensors"},
                new FieldConverterBool<ParamField>(ParamField.NRF24L01_Mode,(sender) =>{  EvtOnPropertychanged?.Invoke (sender.FieldId () ,sender.Data); } ),
                new FieldConverterBool<ParamField>(ParamField.CC2500_Mode,(sender) =>{  EvtOnPropertychanged?.Invoke (sender.FieldId () ,sender.Data); })
            };
        }

        #endregion

        #region Private Fields
        private string portName = "/dev/ttyUSB0";
        private int baudrate = 57600;
        private SerialPortInput serialPort;
        private DateTime lastReceivedData;
        private DateTime? lastReceivedStatus;
        private DateTime lastTryConnection;
        private bool parametersSended;
        private Timer watchDog;
        List<RFLinkItem> listMember = new List<RFLinkItem> ();
        private List<IFieldConverter<ParamField>> listProperties;
        #endregion

        #region Properties
        public short TimeoutForDropMember { get; set; }

        private string version;
        public string Version{
            get { return version; }
            private set { 
                version = value;
                EvtOnVersionchanged?.Invoke ();
            }
        }

        private bool debug;
        public bool Debug{
            get { return debug; }
            set { 
                debug = value;
               // if(_serialPort != null) _serialPort.Debug = _debug;
            }
        }

        public string PortName {
            get { return portName; }
            set { portName=value; }
        }

        public bool IsConnected {
            get { return serialPort != null && serialPort.IsConnected; }
        }

        public bool  RFDebug{
            get { return GetPropertyByEnum(ParamField.rfdebug); }
            set { 
                SetPropertyByEnum (ParamField.rfdebug,value);
                if (IsConnected) {
                    serialPort.SendMessage (RFLink_control.RFDebugRequest (value));
                }
            }
        }

        public bool RFuDebug {
            get {return GetPropertyByEnum (ParamField.rfudebug); }
            set {
                SetPropertyByEnum (ParamField.rfudebug, value);
                if (IsConnected) {
                    serialPort.SendMessage (RFLink_control.RFUDebugRequest (value));
                }
            }
        }
        public bool QRFDebug {
            get { return GetPropertyByEnum (ParamField.qrfdebug); }
            set {
                SetPropertyByEnum (ParamField.qrfdebug, value);
                if (IsConnected) {
                    serialPort.SendMessage (RFLink_control.QRFDebugRequest (value));
                }
            }
        }
        public bool RF433 {
            get { return GetPropertyByEnum (ParamField.setRF433); }
            set {
                SetPropertyByEnum (ParamField.setRF433, value);
                if (IsConnected) {
                    serialPort.SendMessage (RFLink_control.RF433Request (value));
                }
            }
        }
        public bool NodoNRF {
            get { return GetPropertyByEnum (ParamField.setNodoNRF); }
            set {
                SetPropertyByEnum (ParamField.setNodoNRF, value);
                if (IsConnected) {
                    serialPort.SendMessage (RFLink_control.NodoNRFRequest (value));
                }
            }
        }

        private NRF24L01Mode? nrf24L01Mode;
        private NRF24L01Mode? lastnrf24L01Mode;
        public NRF24L01Mode NRF24L01Mode{
            get {
                    if (!IsConnected) { return nrf24L01Mode??NRF24L01Mode.Disable; }
                    int ret = 0;
                    ret = ret | ((MiLight ? 1 : 0) << 0);
                    ret = ret | ((BLE ? 1 : 0) << 1);
                    ret = ret | ((MySensor ? 1 : 0) << 2);
                    var tret = (NRF24L01Mode)ret;
                    if (nrf24L01Mode == null || nrf24L01Mode != tret) {
                        nrf24L01Mode = tret;
                        EvtOnPropertychanged?.Invoke (ParamField.NRF24L01_Mode, nrf24L01Mode);
                    }
                    return tret; 
                }
            set{
                lastnrf24L01Mode = value;

                var tmp1 = (((int)value & (1 << 0)) != 0);
                var tmp2 = (((int)value & (1 << 1)) != 0);
                var tmp3 = (((int)value & (1 << 2)) != 0);

                //Update all false in first 
                var mem1 = MiLight;
                var mem2 = BLE;
                var mem3 = MySensor;

                ////Update all false in first 
                //MiLight = tmp1;
                //BLE = tmp2;
                //MySensor = tmp3;

                if (nrf24L01Mode == null || nrf24L01Mode != value) {
                    nrf24L01Mode = value;
                    EvtOnPropertychanged?.Invoke (ParamField.NRF24L01_Mode, nrf24L01Mode);
                }

               
                //Send Reset value before active mode
                if (IsConnected) 
                {
                    //Disable condition
                    if(!(tmp1|tmp2|tmp3)){
                        if (mem1) serialPort.SendMessage (RFLink_control.MiLightRequest (tmp1));
                        if (mem2) serialPort.SendMessage (RFLink_control.BLERequest (tmp2));
                        if (mem3) serialPort.SendMessage (RFLink_control.MySensorRequest (tmp3));
                    }else{
                        if (tmp1) serialPort.SendMessage (RFLink_control.MiLightRequest (tmp1));
                        if (tmp2) serialPort.SendMessage (RFLink_control.BLERequest (tmp2));
                        if (tmp3) serialPort.SendMessage (RFLink_control.MySensorRequest (tmp3));
                    }
                }
			}
        }

        private bool MiLight {
            get { return GetPropertyByEnum (ParamField.setMilight); }
            set {
                if(value){
                    SetPropertyByEnum (ParamField.setBLE, false);
                    SetPropertyByEnum (ParamField.setMysensors, false);
                }
                SetPropertyByEnum (ParamField.setMilight,value);
            }
        }
        private bool BLE {
            get { return GetPropertyByEnum (ParamField.setBLE); }
            set {
                if (value) {
                    SetPropertyByEnum (ParamField.setMilight, false);
                    SetPropertyByEnum (ParamField.setMysensors, false);
                }
                SetPropertyByEnum (ParamField.setBLE, value);
            }
        }
        private bool MySensor {
            get { return GetPropertyByEnum (ParamField.setMysensors); }
            set {
                if (value) {
                    SetPropertyByEnum (ParamField.setMilight, false);
                    SetPropertyByEnum (ParamField.setBLE, false);
                }
                SetPropertyByEnum (ParamField.setMysensors, value);
            }
        }

        private CC2500Mode? cc2500Mode;
        private CC2500Mode? lastcc2500Mode;
        public CC2500Mode CC2500Mode {
            get {
                    if (!IsConnected) { return cc2500Mode ?? CC2500Mode.Disable; }
                    int ret = 0;
                    ret = ret | (livingColors ? 1 : 0 << 0);
                    ret = ret | (ansluta ? 1 : 0 << 1);

                    var tret = (CC2500Mode)ret;
                    if (cc2500Mode == null || cc2500Mode != tret) {
                        cc2500Mode = tret;
                        EvtOnPropertychanged?.Invoke (ParamField.CC2500_Mode, cc2500Mode);
                    }
                    return tret;
                }
            set {
                lastcc2500Mode = value;

                //Update all false in first
                var tmp1 = (((int)value & (1 << 0)) != 0);
                var tmp2 = (((int)value & (1 << 1)) != 0);
                //Update all false in first 
                var mem1 = livingColors;
                var mem2 = ansluta;

                ////Update all false in first 
                //livingColors = tmp1;
                //ansluta = tmp2;

                if (cc2500Mode == null || cc2500Mode != value) {
                    cc2500Mode = value;
                    EvtOnPropertychanged?.Invoke (ParamField.CC2500_Mode, cc2500Mode);
                }

                //Send Reset value before active mode
                if (IsConnected) {
                    //Disable condition
                    if (!(tmp1 | tmp2 )) {
                        if (mem1) serialPort.SendMessage (RFLink_control.LivingColorsRequest (tmp1));
                        if (mem2) serialPort.SendMessage (RFLink_control.AnslutaRequest (tmp2));
                    }else{
                        if (tmp1) serialPort.SendMessage (RFLink_control.LivingColorsRequest (tmp1));
                        if (tmp2) serialPort.SendMessage (RFLink_control.AnslutaRequest (tmp2));
                    }
                }
            }
        }

        private bool livingColors {
            get { return GetPropertyByEnum (ParamField.setLivingColors); }
            set {
                if (value) {
                    SetPropertyByEnum (ParamField.setAnsluta, false);
                }
                SetPropertyByEnum (ParamField.setLivingColors, value);
            }
        }
        private bool ansluta {
            get { return GetPropertyByEnum (ParamField.setAnsluta); }
            set {
                if (value) {
                    SetPropertyByEnum (ParamField.setLivingColors, false);
                }
                SetPropertyByEnum (ParamField.setAnsluta, value);
            }
        }

        public bool GPIO {
            get { return GetPropertyByEnum (ParamField.setGPIO); }
            set {
                SetPropertyByEnum (ParamField.setGPIO, value);
                if (IsConnected) {
                    serialPort.SendMessage (RFLink_control.GPIORequest (value));
                }
            }
        }
        #endregion

        #region Publics Events
        public delegate void OnDatachanged(string id, DataField type, Object data);
        public event OnDatachanged EvtOnDatachanged;

        public delegate void OnPropertychanged (ParamField type, Object data);
        public event OnPropertychanged EvtOnPropertychanged;

        public delegate void OnItemchanged ();
        public event OnItemchanged EvtOnVersionchanged;

        public delegate void OnItemCreated (string id);
        public event OnItemCreated EvtOnItemCreated;

        //Item is deleted if the last request is very old
        public delegate void OnItemDeleted (string id);
        public event OnItemDeleted EvtOnItemDeleted;

        //Item is deleted if the last request is very old
        public delegate void LogHandle (string id);
        public event LogHandle EvtLog;
        #endregion

        #region Privates Events & Decoding function 
        void serialPortConnectionStatusChanged (object sender, ConnectionStatusChangedEventArgs args)
        {
            //If state is now connected
            if (args?.Connected ?? false) {
                lastReceivedData = DateTime.Now;
                lastReceivedStatus = DateTime.Now;
                _partrequest = "";
                parametersSended = false;
            }
        }

        private string _partrequest;
        void serialPortMessageReceived (object sender, MessageReceivedEventArgs args)
        {
            if (args == null) return;
            string ret;
            try {
                //Update de time of last request
                var _now = DateTime.Now;
                if (DateTime.Compare (lastReceivedData.AddMilliseconds (1000), _now) < 0) {
                    _partrequest = "";
                }
                lastReceivedData = _now;

                ret = _partrequest + System.Text.Encoding.UTF8.GetString (args.Data);
                logTrace ($"Serial Port Received : {ret} ");


                //Separate each request
                var tab = ret.Split (new string [] { "\r\n" }, StringSplitOptions.None);
                if (tab.Length > 1) {
                    for (int i = 0; i < tab.Length - 1; i++) {
                        phraseDecode (tab [i]);
                    }
                    //Store the part request
                    _partrequest = tab [tab.Length - 1];
                }
                foreach (string str in tab) {

                }
            } catch (Exception ex) {
                logError (ex);
            }
        }

        private void phraseDecode(string str){
            try{
                if (str == null) return;
                var tabrequest = str.Split (new string [] { ";" }, StringSplitOptions.None);
                if (tabrequest.Length > 1 && tabrequest [0] == "20") {
                    string _verb = tabrequest [2].ToUpper ();
                    if (_verb == "PONG") {
                        //If Pong
                    } else if (_verb == "STATUS") {
                        //IF status
                        setProperties (tabrequest, 3);
                        if(!parametersSended){
                            if(lastnrf24L01Mode != null)NRF24L01Mode =(NRF24L01Mode) lastnrf24L01Mode;
                            if (lastcc2500Mode != null) CC2500Mode =(CC2500Mode) lastcc2500Mode;
                            parametersSended = true;
                        }
                    } else if (_verb.Contains ("=")) {
                        //unique property
                        setProperties (tabrequest, 2);

                    } else if (tabrequest.Length <= 4 ) {
                        if(tabrequest [2].Contains ("Nodo RadioFrequencyLink")){
                            Version = tabrequest [2];
                            //if gateway  restart ask again status
                            lastReceivedStatus = DateTime.Now;
                        }
                        return;
                    } else {
                        // Data Received Section 

                        var _protocol = tabrequest [2];
                        var _id = tabrequest [3].Replace ("ID=", ""); //Delete ID=

                        //Faire une recherche de l'équiepement, si il n'existe pas, il faut le créer
                        var _member = listMember.Find (x => x.Id == RFLinkItem.MakeId (_protocol, _id));
                        if (_member == null) {
                            _member = new RFLinkItem (_protocol, _id);
                            _member.EvtOnDatachanged +=memberEvtOnDatachanged;
                            listMember.Add (_member);
                            EvtOnItemCreated?.Invoke (_member.Id);
                            logTrace ($"Detect firt received of member : {_member.Id} ");
                        }

                        //Ensuite il faut associer la ou les valeurs reçues (celles avec les égales) au propriétés associées.
                        //The last column can be empty, else data is corrupted
                        for (int i = 4; i < tabrequest.Length - 1; i++) {
                            var res = tabrequest [i].Split (new string [] { "=" }, StringSplitOptions.None);
                            if (res.Length == 2 && res [0] != "" & res [1] != "") 
                            {
                                _member.UpdateData (res [0].ToUpper (), res [1]);
                            }
                        }

                    }
                }
            }catch(Exception ex){
               logError (ex);
            }
        }
         
        private void setProperties (string [] arr, int startindex = 0)
        {
            if (startindex >= (arr?.Length ?? 0)) return;
            for (int i = startindex; i < arr.Length; i++) {
                if (arr [i].Contains ("=")) {
                    var res = arr [i].ToLower ().Split (new string [] { "=" }, StringSplitOptions.None);

                    if (res.Length == 2 && res [0] != "" & res [1] != "") {

                        var _field = listProperties.Find (x => x.Name ().ToUpper () == res [0].ToUpper () 
                                                           || x.OtherName.ToUpper () == res [0].ToUpper ());
                        if (_field != null) {
                            _field.UpdateValue (res [1]);
                        }
                    }
                }

            }

        }
        #endregion

        #region Evenements HandlePropertyChanged
        void memberEvtOnDatachanged (RFLink.RFLinkItem sender, RFLink.BaseFieldConverter<RFLink.DataField> data)
        {
            EvtOnDatachanged?.Invoke (sender.Id, data.FieldId(), data.Data);
        }

        void handlePropertyChangedStandard (RFLink.BaseFieldConverter<RFLink.ParamField> sender)
        {
            EvtOnPropertychanged?.Invoke (sender.FieldId(), sender.Data);
        }
       
        void handlePropertyChangedNRF24L01 (RFLink.BaseFieldConverter<RFLink.ParamField> sender,NRF24L01Mode mode)
        {
                //read property to update it
                var a = NRF24L01Mode;
        }
        void handlePropertyChangedCC2500 (RFLink.BaseFieldConverter<RFLink.ParamField> sender,CC2500Mode mode)
        {
            //read property to update it
            var a = CC2500Mode;
        }
        #endregion

        #region Evenements Message Sending
        void rflinkControlEvtLog (string id)
        {
            logTrace ($"Serial Port Send : {id}");
        }
        #endregion

        #region Publics methods
        public void Connect(){
            try {
                if (serialPort != null && !serialPort.IsConnected) {
                    serialPort.SetPort (portName, baudrate);
                    lastTryConnection = DateTime.Now;

                    logTrace ($"Begin try to connect portname : {portName} baudrate : {baudrate}  ");

                    serialPort.Connect ();
                    //Launch Watchdog
                    if (watchDog == null) watchDog = new Timer ((state) => { watchprocess (); }, null, 0, 5000);
                }
            } catch (Exception ex) {
                logError (ex);
            }           
        }

        public void Disconnect ()
        {
            try{
                logTrace ($"Disconnect ");

                serialPort?.Disconnect ();

                watchDog?.Dispose () ;
                watchDog = null;
            }catch (Exception ex) {
                logError (ex);
            }
        }

        public void RebootGateway ()
        {
            try {
                if (IsConnected) {
                    serialPort.SendMessage (RFLink_control.RebootRequest ());
                }
            } catch (Exception ex) {
               logError (ex);
            }
        }


        public void StatusGateway ()
        {
            try {
                if (IsConnected) {
                    serialPort.SendMessage (RFLink_control.StatusRequest ());
                }
            } catch (Exception ex) {
               logError (ex);
            }
        }

        public void SendRemoteCommand (string protocol, string id, string ChanalNumber, Command cmd)
        {
            try 
            {
                    SendRemoteCommand (protocol, id, ChanalNumber, cmd.ToString ());
             
            } catch (Exception ex) {
               logError (ex);
            }   
        }

        public void SendRemoteCommand (string protocol,string id,string ChanalNumber,string cmd)
        {
            try {
                if (IsConnected) {
                    serialPort.SendMessage (RemoteRequest (protocol, id, ChanalNumber, cmd));
                }
            } catch (Exception ex) {
                logError (ex);
            }
        }
        #endregion

        #region Privates methods
        private void watchprocess(){
            try{
                if(IsConnected){

                    if (lastReceivedData != null && DateTime.Compare(lastReceivedData.AddSeconds(20), DateTime.Now) < 0){
						serialPort.Disconnect();
                        serialPort.Connect();
                    }
                    else if ( lastReceivedData != null && DateTime.Compare (lastReceivedData.AddMilliseconds (5000), DateTime.Now) < 0) {
                        //Send LifeSigne
                        serialPort.SendMessage (RFLink_control.PingRequest ());
                    }

                    if ( lastReceivedStatus != null && DateTime.Compare (lastReceivedStatus ?? DateTime.Now.AddMilliseconds (1000), DateTime.Now) < 0) {
                        serialPort.SendMessage (RFLink_control.StatusRequest ());
                        lastReceivedStatus = null;
                    }
                }else if(DateTime.Compare (lastTryConnection.AddMilliseconds (5000), DateTime.Now) < 0) {
                    Connect ();
                }

                //Delete the older members
                foreach(RFLinkItem x in listMember){
                    if( DateTime.Compare (x.LastUpdate.AddMinutes (TimeoutForDropMember), DateTime.Now) < 0){
                        EvtOnItemDeleted?.Invoke (x.Id);
                        x.EvtOnDatachanged -= memberEvtOnDatachanged;
                        logTrace ($"Remove old member Id = {x.Id} LastUpdate : {x.LastUpdate.ToString ()}  ");
                        listMember.Remove (x);
                    }
                }


            }catch (Exception ex){
                logError (ex);
            }
        }
        private bool GetPropertyByEnum (ParamField enu)
        {
            try {
                return (bool)listProperties [(int)enu].Data ;
            } catch (Exception ex) {
                logError (ex);
            }
            return false;
        }
        private void SetPropertyByEnum (ParamField enu,bool data)
        {
            try {
                listProperties [(int)enu].Data =data;
            } catch (Exception ex) {
                logError (ex);
            }
        }


        #endregion

        #region log methods
        private void logTrace (string msg)
        {
            try {
                EvtLog?.Invoke (msg);
                if (debug) Console.WriteLine (msg);
            } catch {

            }
        }

        private void logError (Exception ex, [CallerMemberName] string name = "")
        {
            if (EvtLog == null || debug)
                logTrace ($"Error RFLinkLib : {name} : {ex.Message}");
        }
        #endregion


    }
}
