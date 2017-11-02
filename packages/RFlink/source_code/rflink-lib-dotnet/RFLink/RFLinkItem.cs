using System;
using System.Collections.Generic;

namespace RFLink
{
    public class RFLinkItem
    {
      
        #region Constructor
        public RFLinkItem (string protocol, string id)
        {
            RfProtocol = protocol;
            RfId = id;
            LastUpdate = DateTime.Now;

            //All fields declaration possibility
            lstData = new List<IFieldConverter<DataField>> (new IFieldConverter<DataField> []{
                new FieldConverter<DataField>(DataField.SWITCH,handlePropertyChanged){IgnoreFlood = true},
                    new FieldConverter<DataField>(DataField.CMD,handlePropertyChanged){IgnoreFlood = true},
                    new FieldConverterDec <DataField>(DataField.SET_LEVEL,false,handlePropertyChanged){IgnoreFlood = true},
                    new FieldConverterHexDec<DataField> (DataField.TEMP,true,true,handlePropertyChanged) ,
                    new FieldConverterDec<DataField>   (DataField.HUM,false,handlePropertyChanged),
                    new FieldConverterHexDec<DataField>(DataField.BARO,false,false,handlePropertyChanged),
                    new FieldConverterDecEnum<DataField,HStatus>  (DataField.HSTATUS,handlePropertyChanged),
                    new FieldConverterDecEnum<DataField,BforeCast> (DataField.BFORECAST,handlePropertyChanged),
                    new FieldConverterHexDec <DataField> (DataField.UV,false,false,handlePropertyChanged),
                    new FieldConverterHexDec<DataField>(DataField.LUX,false,false,handlePropertyChanged),
                    new FieldConverter <DataField>   (DataField.BAT,handlePropertyChanged),
                    new FieldConverterHexDec<DataField>   (DataField.RAIN,false,true,handlePropertyChanged),
                    new FieldConverterHexDec<DataField> (DataField.RAINRATE,false,true,handlePropertyChanged),
                    new FieldConverterHexDec<DataField> (DataField.RAINTOT,false,true,handlePropertyChanged),
                    new FieldConverterDec<DataField> (DataField.WINSP,true,handlePropertyChanged),
                    new FieldConverterDec<DataField>  (DataField.AWINSP,true,handlePropertyChanged),
                    new FieldConverterDec <DataField> (DataField.WINGS,false,handlePropertyChanged),
                    new FieldConverterAngular<DataField>(DataField.WINDIR,handlePropertyChanged),
                    new FieldConverterHexDec<DataField> (DataField.WINCHL,true,true,handlePropertyChanged),
                    new FieldConverterHexDec<DataField>  (DataField.WINTMP,true,true,handlePropertyChanged),
                    new FieldConverterDec <DataField> (DataField.CHIME,false,handlePropertyChanged),
                    new FieldConverter <DataField> (DataField.SMOKEALERT,handlePropertyChanged) ,
                    new FieldConverter<DataField>  (DataField.PIR,handlePropertyChanged),
                    new FieldConverterDec <DataField>(DataField.CO2,false,handlePropertyChanged),
                    new FieldConverterDec <DataField> (DataField.SOUND,false,handlePropertyChanged),
                    new FieldConverterHexDec<DataField> (DataField.KWATT,false,false,handlePropertyChanged),
                    new FieldConverterHexDec<DataField> (DataField.WATT,false,false,handlePropertyChanged),
                    new FieldConverterDec<DataField>  (DataField.CURRENT,false,handlePropertyChanged),
                    new FieldConverterDec<DataField> (DataField.CURRENT2,false,handlePropertyChanged),
                    new FieldConverterDec<DataField> (DataField.CURRENT3,false,handlePropertyChanged),
                    new FieldConverterDec<DataField>  (DataField.DIST,false,handlePropertyChanged),
                    new FieldConverterDec<DataField>  (DataField.METER,false,handlePropertyChanged),
                    new FieldConverterDec <DataField> (DataField.VOLT,false,handlePropertyChanged),
                    new FieldConverterHexDec <DataField>(DataField.RGBW,false,false,handlePropertyChanged)
                });

            listChangedProperties = new List<DataField> ();
        }
        #endregion

        #region Publics Events

        public delegate void OnDatachanged (RFLinkItem sender, RFLink.BaseFieldConverter<RFLink.DataField> data);
        public event OnDatachanged EvtOnDatachanged;

        void handlePropertyChanged (RFLink.BaseFieldConverter<RFLink.DataField> sender)
        {
            EvtOnDatachanged?.Invoke (this, sender);
        }
        #endregion

        #region private fields
        private List<IFieldConverter<DataField>> lstData;
        private List<DataField> listChangedProperties;
        #endregion

        #region properties
        /// <summary>
        /// Identifiant pour Homegenie
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get { return MakeId (RfProtocol, RfId); } }
        /// <summary>
        /// Date et heure de la dernière mise à jour
        /// </summary>
        /// <value>The last update.</value>
        public DateTime LastUpdate { get; private set; }

        /// <summary>
        /// Nom du protocol de l'item
        /// </summary>
        /// <value>The rf protocol.</value>
        public string RfProtocol { get; private set; }
        /// <summary>
        /// Id de l'item (en fonction du protocol)
        /// </summary>
        /// <value>The rf identifier.</value>
        public string RfId { get; private set; }
        #endregion

        #region public methodss
        public IFieldConverter<DataField> UpdateData (string name, string content)
        {

            var _field = lstData.Find (x => x.Name () == name);
            if (_field != null) {
                //If this item changed not, return null value  but updated last communication property
                LastUpdate = DateTime.Now;
                var _firstupd = (_field.Data == null);
                if (_field.UpdateValue (content)) {
                    if (_firstupd) listChangedProperties.Add (_field.FieldId ());
                    return _field;
                }
            } else {
                //Field not found
            }
            return null;
        }

        public List<IFieldConverter<DataField>> GetAllUsedData ()
        {
            var _list = new List<IFieldConverter<DataField>> ();
            foreach (DataField itm in listChangedProperties) {
                _list.Add (lstData [(int)itm]);
            }
            return _list;
        }
        #endregion

        #region static methods
        public static string MakeId (string protocol, string id)
        {
            return protocol + "-" + id;
        }
        #endregion

    }
}
