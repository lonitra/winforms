// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;

namespace System.Windows.Forms;

// This choice would not be trim friendly.. We cannot make this <T> without making DataObject serializable.
// This is because when we use IObjectReference to deserialize (similar to JsonData approach), it will check
// whether DataObject is serializable. We do not want to put serializable on DataObject as it is public API.

// Additionally, this presents an issue because when we get this off of the clipboard, we lose all information
// that it was originally supposed to be a JsonDataObject. In GetDataObject, we get some object from PInvoke,
// where we cannot assume that the object was a JsonDataObject because then if it is just a normal string that was
// serialized and not necessary a Json string, we would likely get unexpected behavior. Essentially, JsonDataObject
// does not help us differentiate whether the actual data was a normal string or a Json string. We could make this work by
// wrapping JsonDataObject in a DataObject when we set the data on the clipboard, but this would defeat the purpose of
// subclassing DataObject in the first place. At this point it would be less complicated to go with JsonData approach.
[Serializable]
internal class JsonDataObject : DataObject
{
    internal string FullyQualifiedTypeName { get; private set; }
    internal string JsonData { get; private set; }

    public JsonDataObject(object data) : base()
    {
        JsonData = JsonSerializer.Serialize(data);
        SetData(JsonData);
        FullyQualifiedTypeName = data.GetType().AssemblyQualifiedName!;
    }

    public override object? GetData(string format, bool autoConvert)
    {
        object? data = base.GetData(format, autoConvert);
        if (data is not string jsonString)
        {
            return null;
        }

        return JsonSerializer.Deserialize(jsonString, Type.GetType(FullyQualifiedTypeName));
    }

    /*    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(FullyQualifiedTypeName), FullyQualifiedTypeName);
            info.AddValue(nameof(JsonData), JsonData);
        }*/
}
