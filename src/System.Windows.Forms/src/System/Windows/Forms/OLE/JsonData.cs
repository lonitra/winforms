// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;
using System.Text.Json;

namespace System.Windows.Forms;

// make this JsonDataObject<T>, rename to JsonData -- add binder to binaryformattedobject when creating
[Serializable]
internal struct JsonData<T> : IObjectReference, JsonData
{
    public string JsonString { get; set; }

    //public readonly string? TypeFullName => typeof(T).FullName!;

    //public string FullyQualifiedTypeName { get; set; }

    public readonly object GetRealObject(StreamingContext context) =>
        JsonSerializer.Deserialize(JsonString, typeof(T)) ?? throw new InvalidOperationException();
}

// should not create this.. Create JsonData<T>
internal interface JsonData
{
    string JsonString { get; set; }

    //string? TypeFullName { get; }
}
