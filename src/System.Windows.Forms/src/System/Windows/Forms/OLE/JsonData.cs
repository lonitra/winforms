// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.Serialization;
using System.Text.Json;

namespace System.Windows.Forms;

/// <summary>
///  Contains JSON serialized data along with the JSON data's original type information.
/// </summary>
[Serializable]
#pragma warning disable SYSLIB0050 // Type or member is obsolete
internal struct JsonData<T> : IObjectReference, JsonData
#pragma warning restore SYSLIB0050 // Type or member is obsolete
{
    public string JsonString { get; set; }

    public readonly object GetRealObject(StreamingContext context) =>
        JsonSerializer.Deserialize(JsonString, typeof(T)) ?? throw new InvalidOperationException();
}

/// <summary>
///  Represents an object that contains JSON serialized data.
/// </summary>
internal interface JsonData
{
    string JsonString { get; set; }
}
