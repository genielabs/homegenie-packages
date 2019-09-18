using System;
namespace RFLink
{

    public class FieldConverterHexDec<T> : BaseFieldConverter<T>, IFieldConverter<T>
    where T : struct, IConvertible, IComparable, IFormattable
    {
        public FieldConverterHexDec (T id , bool withsign = false, bool withdivid = false, SimpleCallBack callbackpropertychange = null) : base (id, callbackpropertychange)
        {
            withSign = withsign;
            withDivid = withdivid;
        }

        private bool withSign;
        private bool withDivid;
        public bool UpdateValue (string data)
        {
            Int16 decValue = Convert.ToInt16 (data, 16);
            if (withSign) {
                Int16 decmask = 0x7FFF;
                decValue = (Int16)(decValue & decmask);
                if ((decValue >> 15) != 0) {
                    decValue = (Int16)(decValue * -1);
                }
            }
            if (withDivid) {
                this.Data = ((float)decValue / (float)10);
            }else{
                this.Data = decValue;
            }

            return true;
        }
    }

}
