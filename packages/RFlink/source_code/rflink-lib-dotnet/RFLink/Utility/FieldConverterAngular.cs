using System;
namespace RFLink
{

    public class FieldConverterAngular<T> : BaseFieldConverter<T>, IFieldConverter<T>
    where T : struct, IConvertible, IComparable, IFormattable
    {
        public FieldConverterAngular (T id ,SimpleCallBack callbackpropertychange = null) : base (id, callbackpropertychange)
        {
        }
        public bool UpdateValue (string data)
        {

            Int16 decValue = Convert.ToInt16 (data);
            this.Data = (((float)decValue * (float)22.5) % 360);//.ToString ("0.00");
            return true;
        }
    }
}
