﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;

namespace System.Windows.Forms.BinaryFormat;

internal static class WinFormsBinaryFormattedObjectExtensions
{
    /// <summary>
    ///  Tries to get this object as a binary formatted <see cref="ImageListStreamer"/>.
    /// </summary>
    public static bool TryGetImageListStreamer(
        this BinaryFormattedObject format,
        out object? imageListStreamer)
    {
        return BinaryFormattedObjectExtensions.TryGet(Get, format, out imageListStreamer);

        static bool Get(BinaryFormattedObject format, [NotNullWhen(true)] out object? imageListStreamer)
        {
            imageListStreamer = null;

            if (format.RootRecord is not ClassWithMembersAndTypes types
                || types.ClassInfo.Name != typeof(ImageListStreamer).FullName
                || format[3] is not ArraySinglePrimitive<byte> data)
            {
                return false;
            }

            Debug.Assert(data.ArrayObjects is byte[]);
            imageListStreamer = new ImageListStreamer((byte[])data.ArrayObjects);
            return true;
        }
    }

    /// <summary>
    ///  Tries to get this object as a binary formatted <see cref="Bitmap"/>.
    /// </summary>
    public static bool TryGetBitmap(this BinaryFormattedObject format, out object? bitmap)
    {
        bitmap = null;

        if (format.RootRecord is not ClassWithMembersAndTypes types
            || types.ClassInfo.Name != typeof(Bitmap).FullName
            || format[3] is not ArraySinglePrimitive<byte> data)
        {
            return false;
        }

        Debug.Assert(data.ArrayObjects is byte[]);
        bitmap = new Bitmap(new MemoryStream((byte[])data.ArrayObjects));
        return true;
    }

    /// <summary>
    ///  Tries to deserialize this object if it was serialized as JSON.
    /// </summary>
    public static bool TryGetObjectFromJson(this BinaryFormattedObject format, out object? @object)
    {
        @object = null;

        if (format.RootRecord is not ClassWithMembersAndTypes types
            || !types.ClassInfo.Name.Contains(typeof(JsonData).FullName))
        {
            // The data was not serialized as JSON.
            return false;
        }

        @object = format.Deserialize();
        return true;
    }

    /// <summary>
    ///  Try to get a supported object.
    /// </summary>
    public static bool TryGetObject(this BinaryFormattedObject format, [NotNullWhen(true)] out object? value) =>
        format.TryGetFrameworkObject(out value)
        || format.TryGetBitmap(out value)
        || format.TryGetImageListStreamer(out value)
        || format.TryGetObjectFromJson(out value);
}
