// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;
using System.Text.Json;

namespace System.Windows.Forms.BinaryFormat;

/// <summary>
///  Writer that writes Windows Forms specific types in binary format without using the BinaryFormatter.
/// </summary>
internal static class WinFormsBinaryFormatWriter
{
    private static readonly string[] s_dataMemberName = ["Data"];

    private static readonly string s_currentWinFormsFullName = typeof(WinFormsBinaryFormatWriter).Assembly.FullName!;

/*    public static unsafe void WriteJsonStream(Stream stream, object @object)
    {
        JsonData jsonDataObject = new()
        {
            JsonString = JsonSerializer.Serialize(@object),
            FullyQualifiedTypeName = @object.GetType().AssemblyQualifiedName!
        };

        using BinaryFormatWriterScope writer = new(stream);
        new BinaryLibrary(2, s_currentWinFormsFullName).Write(writer);
        new ClassWithMembersAndTypes(
            new ClassInfo(1, typeof(JsonData).FullName!, ["<JsonString>k__BackingField", "<FullyQualifiedTypeName>k__BackingField"]),
            libraryId: 2,
            new MemberTypeInfo((BinaryType.String, null), (BinaryType.String, null)),
            new BinaryObjectString(3, jsonDataObject.JsonString),
            new BinaryObjectString(4, jsonDataObject.FullyQualifiedTypeName)).Write(writer);
    }*/

    public static unsafe void WriteJsonStream(Stream stream, JsonData jsonData)
    {
        using BinaryFormatWriterScope writer = new(stream);
        new BinaryLibrary(2, s_currentWinFormsFullName).Write(writer);
        new ClassWithMembersAndTypes(
            new ClassInfo(1, jsonData.GetType().FullName!, ["<JsonString>k__BackingField"]),
            libraryId: 2,
            new MemberTypeInfo((BinaryType.String, null)),
            new BinaryObjectString(3, jsonData.JsonString)).Write(writer);
            //new BinaryObjectString(4, jsonDataObject.FullyQualifiedTypeName)).Write(writer);
    }

    public static unsafe void WriteJsonDataObject(Stream stream, JsonDataObject jsonDataObject)
    {
        using BinaryFormatWriterScope writer = new(stream);
        new BinaryLibrary(2, s_currentWinFormsFullName).Write(writer);
        new ClassWithMembersAndTypes(
            new ClassInfo(1, jsonDataObject.GetType().FullName!, [nameof(jsonDataObject.FullyQualifiedTypeName), nameof(jsonDataObject.JsonData)]),
            libraryId: 2,
            new MemberTypeInfo((BinaryType.String, null), (BinaryType.String, null)),
            new BinaryObjectString(3, jsonDataObject.FullyQualifiedTypeName),
            new BinaryObjectString(4, jsonDataObject.JsonData)).Write(writer);
    }

    public static unsafe void WriteBitmap(Stream stream, Bitmap bitmap)
    {
        using MemoryStream memoryStream = new();
        bitmap.Save(memoryStream);

        bool success = memoryStream.TryGetBuffer(out ArraySegment<byte> data);
        Debug.Assert(success);

        using BinaryFormatWriterScope writer = new(stream);
        new BinaryLibrary(2, AssemblyRef.SystemDrawing).Write(writer);
        new ClassWithMembersAndTypes(
            new ClassInfo(1, typeof(Bitmap).FullName!, s_dataMemberName),
            libraryId: 2,
            new MemberTypeInfo((BinaryType.PrimitiveArray, PrimitiveType.Byte)),
            new MemberReference(3)).Write(writer);

        new ArraySinglePrimitive<byte>(3, data).Write(writer);
    }

    public static void WriteImageListStreamer(Stream stream, ImageListStreamer streamer)
    {
        byte[] data = streamer.Serialize();

        using BinaryFormatWriterScope writer = new(stream);

        new BinaryLibrary(2, s_currentWinFormsFullName).Write(writer);
        new ClassWithMembersAndTypes(
            new ClassInfo(1, typeof(ImageListStreamer).FullName!, s_dataMemberName),
            libraryId: 2,
            new MemberTypeInfo((BinaryType.PrimitiveArray, PrimitiveType.Byte)),
            new MemberReference(3)).Write(writer);

        new ArraySinglePrimitive<byte>(3, data).Write(writer);
    }

    /// <summary>
    ///  Writes the given <paramref name="value"/> if supported.
    /// </summary>
    public static bool TryWriteObject(Stream stream, object value)
    {
        // Framework types are more likely to be written, so check them first.
        return BinaryFormatWriter.TryWriteFrameworkObject(stream, value)
            || BinaryFormatWriter.TryWrite(Write, stream, value);

        static bool Write(Stream stream, object value)
        {
            if (value is ImageListStreamer streamer)
            {
                WriteImageListStreamer(stream, streamer);
                return true;
            }
            else if (value is Bitmap bitmap)
            {
                WriteBitmap(stream, bitmap);
                return true;
            }
/*            else if (value is IJsonSerializable jsonSerializable)
            {
                WriteJsonStream(stream, jsonSerializable);
                return true;
            }*/
            else if (value is JsonData jsonData)
            {
                WriteJsonStream(stream, jsonData);
                return true;
            }
            else if (value is JsonDataObject jsonDataObject)
            {
                WriteJsonDataObject(stream, jsonDataObject);
                return true;
            }

            return false;
        }
    }
}
