using System;
namespace RFLink
{
   
    public class FieldConverter<T> : BaseFieldConverter<T>, IFieldConverter<T>
    where T : struct, IConvertible, IComparable, IFormattable
    {
        public FieldConverter (T id , SimpleCallBack callbackpropertychange = null) : base (id, callbackpropertychange)
        {
        }


        public bool UpdateValue (string data)
        {
                this.Data = data;
                return true;
        }
    }

}
