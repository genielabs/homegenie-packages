using System;
namespace RFLink
{
   
    public abstract class BaseFieldConverter <T>
    where T : struct, IConvertible, IComparable, IFormattable
    {
        public BaseFieldConverter (T id, SimpleCallBack callbackpropertychange = null)
        {
            if(OtherName == null) OtherName = "";
            fieldid = id;
            name = id.ToString ();
            callBackPropertyChange = callbackpropertychange;
        }

        public delegate void SimpleCallBack (BaseFieldConverter<T> sender);
        private SimpleCallBack callBackPropertyChange;

        private T fieldid;
        public T FieldId ()
        {
            return fieldid;
        }

        private string name;
        public string Name ()
        {
            return name;
        }


        public string OtherName { get; set; }

        public bool IgnoreFlood { get; set; }


        private object data;
        public object Data { 
            get{
                return data;
            } 
            set{
                try{
                    if (IgnoreFlood || data?.ToString () != value?.ToString ()){
                        data = value;
                        callBackPropertyChange?.Invoke (this);
                    }
                }catch (Exception ex ){
                    var error = ex.Message;
                }

            } 
        }

    }

}
