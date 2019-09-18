using System;
namespace RFLink
{
   
   
    public class FieldConverterDec<T> : BaseFieldConverter<T>, IFieldConverter<T>
    where T : struct, IConvertible, IComparable, IFormattable
    {
        public FieldConverterDec (T id ,  bool withdivid = false, SimpleCallBack callback_propertychange = null) : base (id, callback_propertychange)
        {
            withDivid = withdivid;
        }

        private bool withDivid;

        public bool UpdateValue (string data)
        {
            Int16 decValue = Convert.ToInt16 (data);
            if (withDivid) {
                this.Data = ((float)decValue / (float)10);//.ToString ("0.00");
            }else{
                this.Data = decValue.ToString ();
            }
            return true;
        }
    }
}
