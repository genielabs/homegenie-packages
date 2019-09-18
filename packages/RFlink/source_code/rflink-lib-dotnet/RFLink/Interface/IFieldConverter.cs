using System;
namespace RFLink
{
    public interface IFieldConverter<T>
    where T : struct, IConvertible, IComparable, IFormattable
    {
        T FieldId ();

        string Name ();

        string OtherName { get; set; }

        bool IgnoreFlood { get; set; }

        object Data { get; set; }

        //object Data ();
        //void Data (object data);

        bool UpdateValue (string data);

    }

}
