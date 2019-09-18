using System;
namespace RFLink
{
   
    public class FieldConverterDecEnum<T,TEnum> : BaseFieldConverter<T>, IFieldConverter<T>
    where T : struct, IConvertible, IComparable, IFormattable
    where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        public FieldConverterDecEnum (T id , SimpleCallBack callbackpropertychange = null) : base (id, callbackpropertychange)
        {
        }
        public bool UpdateValue (string data)
        {
            Int16 decValue = Convert.ToInt16 (data);
            this.Data = Enum.GetName (typeof (TEnum), decValue);
            return true;
        }
    }

}
