using System;
namespace RFLink
{
    public class FieldConverterBool<T> : BaseFieldConverter<T>, IFieldConverter<T>
    where T : struct, IConvertible, IComparable, IFormattable
    {
        public FieldConverterBool (T id , SimpleCallBack callbackpropertychange = null) : base (id, callbackpropertychange)
        {
        }


        public bool UpdateValue (string data)
        {
            this.Data =  data.ToUpper () == "ON" ? true : false; 
            return true;
        }
    }
}
