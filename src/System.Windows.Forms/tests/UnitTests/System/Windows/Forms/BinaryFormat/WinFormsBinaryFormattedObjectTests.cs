﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace System.Windows.Forms.BinaryFormat.Tests;

public class WinFormsBinaryFormattedObjectTests
{
    private static readonly Attribute[] s_visible = [DesignerSerializationVisibilityAttribute.Visible];

    [Serializable]
    public class WeatherForecast
    {
        public DateTimeOffset Date { get; set; }
        public int TemperatureCelsius { get; set; }
        public string? Summary { get; set; }
    }

    [Fact]
    public void BinaryFormattedObject_Json_FromBinaryFormatter()
    {
        WeatherForecast weather = new()
        {
            Date = DateTimeOffset.UtcNow,
            TemperatureCelsius = 25,
            Summary = "Hot"
        };

        JsonData<WeatherForecast> json = new()
        {
            JsonString = JsonSerializer.Serialize(weather),
            //FullyQualifiedTypeName = typeof(WeatherForecast).AssemblyQualifiedName!
        };

        BinaryFormattedObject format = json.SerializeAndParse();
        ClassWithMembersAndTypes root = format.RootRecord.Should().BeOfType<ClassWithMembersAndTypes>().Subject;
        root.Name.Should().Be(typeof(JsonData<WeatherForecast>).FullName);
        root["<JsonString>k__BackingField"].Should().BeOfType<BinaryObjectString>();
        //root["<FullyQualifiedTypeName>k__BackingField"].Should().BeOfType<BinaryObjectString>();
        format.TryGetObjectFromJson(out object? result).Should().BeTrue();
        result.Should().BeOfType<WeatherForecast>();
        result.Should().BeEquivalentTo(weather);
    }

    [Fact]
    public void BinaryFormattedObject_Json_RoundTrip()
    {
        WeatherForecast weather = new()
        {
            Date = DateTimeOffset.UtcNow,
            TemperatureCelsius = 25,
            Summary = "Hot"
        };

        JsonData<WeatherForecast> json = new()
        {
            JsonString = JsonSerializer.Serialize(weather),
            //FullyQualifiedTypeName = typeof(WeatherForecast).AssemblyQualifiedName!
        };

        using MemoryStream stream = new();
        WinFormsBinaryFormatWriter.WriteJsonStream(stream, json);

        stream.Position = 0;
        BinaryFormattedObject binary = new(stream);

        binary.TryGetObjectFromJson(out object? result).Should().BeTrue();
        WeatherForecast deserialized = result.Should().BeOfType<WeatherForecast>().Which;
        deserialized.TemperatureCelsius.Should().Be(25);
        deserialized.Summary.Should().Be("Hot");
    }

    [Fact]
    public void BinaryFormattedObject_JsonDataObject_FromBinaryFormatter()
    {
        WeatherForecast weather = new()
        {
            Date = DateTimeOffset.UtcNow,
            TemperatureCelsius = 25,
            Summary = "Hot"
        };

        JsonDataObject json = new(weather);

        BinaryFormattedObject format = json.SerializeAndParse();
        ClassWithMembersAndTypes root = format.RootRecord.Should().BeOfType<ClassWithMembersAndTypes>().Subject;
        root.Name.Should().Be(typeof(JsonData<WeatherForecast>).FullName);
        root["<JsonString>k__BackingField"].Should().BeOfType<BinaryObjectString>();
        //root["<FullyQualifiedTypeName>k__BackingField"].Should().BeOfType<BinaryObjectString>();
        format.TryGetObjectFromJson(out object? result).Should().BeTrue();
        result.Should().BeOfType<WeatherForecast>();
        result.Should().BeEquivalentTo(weather);
    }

    [StaFact]
    public void Clipboard_Json_RoundTrip()
    {
        WeatherForecast weather = new()
        {
            Date = DateTimeOffset.UtcNow,
            TemperatureCelsius = 25,
            Summary = "Hot"
        };

        Clipboard.SetDataAsJson("weather", weather);
        WeatherForecast deserialized = Clipboard.GetData("weather").Should().BeOfType<WeatherForecast>().Which;
        deserialized.TemperatureCelsius.Should().Be(25);
        deserialized.Summary.Should().Be("Hot");
    }

    [StaFact]
    public void Clipboard_JsonData_GetDataObject()
    {
        WeatherForecast weather = new()
        {
            Date = DateTimeOffset.UtcNow,
            TemperatureCelsius = 25,
            Summary = "Hot"
        };

        Clipboard.SetDataAsJson("weather", weather);
        IDataObject? dataObject = Clipboard.GetDataObject();
        dataObject.Should().NotBeNull();
        dataObject.GetDataPresent("weather").Should().BeTrue();
        WeatherForecast deserialized = dataObject.GetData("weather").Should().BeOfType<WeatherForecast>().Which;
        deserialized.TemperatureCelsius.Should().Be(25);
        deserialized.Summary.Should().Be("Hot");
    }

    [StaFact]
    public void Clipboard_JsonDataObject_RoundTrip()
    {
        WeatherForecast weather = new()
        {
            Date = DateTimeOffset.UtcNow,
            TemperatureCelsius = 25,
            Summary = "Hot"
        };

        Clipboard.SetDataAsJsonDataObject("weather", weather);
        WeatherForecast deserialized = Clipboard.GetData("weather").Should().BeOfType<WeatherForecast>().Which;
        deserialized.TemperatureCelsius.Should().Be(25);
        deserialized.Summary.Should().Be("Hot");
    }

    [StaFact]
    public void test()
    {
        using BinaryFormatterScope formatterScope = new(enable: true);
        WeatherForecast weather = new()
        {
            Date = DateTimeOffset.UtcNow,
            TemperatureCelsius = 25,
            Summary = "Hot"
        };

        Clipboard.SetData("weather", new DataObject(weather));
        WeatherForecast deserialized = Clipboard.GetData("weather").Should().BeOfType<WeatherForecast>().Which;
        deserialized.TemperatureCelsius.Should().Be(25);
        deserialized.Summary.Should().Be("Hot");
    }

    [Fact]
    public void BinaryFormattedObject_Bitmap_FromBinaryFormatter()
    {
        using Bitmap bitmap = new(10, 10);
        BinaryFormattedObject format = bitmap.SerializeAndParse();
        ClassWithMembersAndTypes root = format.RootRecord.Should().BeOfType<ClassWithMembersAndTypes>().Subject;
        root.Name.Should().Be(typeof(Bitmap).FullName);
        format[root.LibraryId].Should().BeOfType<BinaryLibrary>().Which
            .LibraryName.Should().Be(AssemblyRef.SystemDrawing);
        MemberReference reference = root["Data"].Should().BeOfType<MemberReference>().Subject;
        format[reference].Should().BeOfType<ArraySinglePrimitive<byte>>();
        format.TryGetBitmap(out object? result).Should().BeTrue();
        using Bitmap deserialized = result.Should().BeOfType<Bitmap>().Which;
        deserialized.Size.Should().Be(bitmap.Size);
    }

    [Fact]
    public void BinaryFormattedObject_Bitmap_RoundTrip()
    {
        using Bitmap bitmap = new(10, 10);
        using MemoryStream stream = new();
        WinFormsBinaryFormatWriter.WriteBitmap(stream, bitmap);

        stream.Position = 0;
        BinaryFormattedObject binary = new(stream);

        binary.TryGetBitmap(out object? result).Should().BeTrue();
        using Bitmap deserialized = result.Should().BeOfType<Bitmap>().Which;
        deserialized.Size.Should().Be(bitmap.Size);
    }

    [Fact]
    public void BinaryFormattedObject_Bitmap_FromWinFormsBinaryFormatWriter()
    {
        using Bitmap bitmap = new(10, 10);
        using MemoryStream stream = new();
        WinFormsBinaryFormatWriter.WriteBitmap(stream, bitmap);

        stream.Position = 0;

        using BinaryFormatterScope formatterScope = new(enable: true);
#pragma warning disable SYSLIB0011 // Type or member is obsolete
        // cs/binary-formatter-without-binder
        BinaryFormatter binaryFormat = new(); // CodeQL [SM04191] This is a test deserialization process is performed on trusted data and the types are controlled and validated.
#pragma warning restore SYSLIB0011

        // cs/dangerous-binary-deserialization
        using Bitmap deserialized = binaryFormat.Deserialize(stream).Should().BeOfType<Bitmap>().Which; // CodeQL [SM03722] : Testing legacy feature. This is a safe use of BinaryFormatter because the data is trusted and the types are controlled and validated.
        deserialized.Size.Should().Be(bitmap.Size);
    }

    [Fact]
    public void BinaryFormattedObject_ImageListStreamer_FromBinaryFormatter()
    {
        using ImageList sourceList = new();
        using Bitmap image = new(10, 10);
        sourceList.Images.Add(image);
        using ImageListStreamer stream = sourceList.ImageStream!;

        BinaryFormattedObject format = stream.SerializeAndParse();
        ClassWithMembersAndTypes root = format.RootRecord.Should().BeOfType<ClassWithMembersAndTypes>().Subject;
        root.Name.Should().Be(typeof(ImageListStreamer).FullName);
        format[root.LibraryId].Should().BeOfType<BinaryLibrary>().Which
            .LibraryName.Should().Be(typeof(WinFormsBinaryFormatWriter).Assembly.FullName);
        MemberReference reference = root["Data"].Should().BeOfType<MemberReference>().Subject;
        format[reference].Should().BeOfType<ArraySinglePrimitive<byte>>();

        format.TryGetImageListStreamer(out object? result).Should().BeTrue();
        using ImageListStreamer deserialized = result.Should().BeOfType<ImageListStreamer>().Which;
        using ImageList newList = new();
        newList.ImageStream = deserialized;
        newList.Images.Count.Should().Be(1);
        Bitmap newImage = (Bitmap)newList.Images[0];
        newImage.Size.Should().Be(sourceList.Images[0].Size);
    }

    [Fact]
    public void BinaryFormattedObject_ImageListStreamer_RoundTrip()
    {
        using ImageList sourceList = new();
        using Bitmap image = new(10, 10);
        sourceList.Images.Add(image);
        using ImageListStreamer stream = sourceList.ImageStream!;

        using MemoryStream memoryStream = new();
        WinFormsBinaryFormatWriter.WriteImageListStreamer(memoryStream, stream);
        memoryStream.Position = 0;
        BinaryFormattedObject binary = new(memoryStream);

        binary.TryGetImageListStreamer(out object? result).Should().BeTrue();
        using ImageListStreamer deserialized = result.Should().BeOfType<ImageListStreamer>().Which;
        using ImageList newList = new();
        newList.ImageStream = deserialized;
        newList.Images.Count.Should().Be(1);
        Bitmap newImage = (Bitmap)newList.Images[0];
        newImage.Size.Should().Be(sourceList.Images[0].Size);
    }

    [Theory]
    [MemberData(nameof(BinaryFormattedObjects_TestData))]
    public void BinaryFormattedObjects_SuccessfullyParse(object value)
    {
        // Check that we can parse types that would hit the BinaryFormatter for property serialization.
        using (value as IDisposable)
        {
            var format = value.SerializeAndParse();
        }
    }

    public static TheoryData<object> BinaryFormattedObjects_TestData => new()
    {
        new PointF(),
        new PointF[] { default },
        new RectangleF(),
        new RectangleF[] { default },
        new DateTime[] { default },
        new ImageListStreamer(new ImageList()),
        new ListViewGroup(),
        new ListViewItem(),
        new OwnerDrawPropertyBag(),
        new TreeNode(),
        new ListViewItem.ListViewSubItem()
    };

    [WinFormsTheory]
    [MemberData(nameof(Control_DesignerVisibleProperties_TestData))]
    public void Control_BinaryFormatted_DesignerVisibleProperties(object value, string[] properties)
    {
        // Check WinForms types for properties that can hit the BinaryFormatter

        using (value as IDisposable)
        {
            var propertyDescriptors = TypeDescriptor.GetProperties(value, s_visible);

            List<string> binaryFormattedProperties = new();
            foreach (PropertyDescriptor property in propertyDescriptors)
            {
                Type propertyType = property.PropertyType;
                if (propertyType.IsBinaryFormatted())
                {
                    binaryFormattedProperties.Add($"{property.Name}: {propertyType.Name}");
                }
            }

            Assert.Equal(properties, binaryFormattedProperties);
        }
    }

    public static TheoryData<object, string[]> Control_DesignerVisibleProperties_TestData => new()
    {
        { new Control(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new Form(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new Button(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new CheckBox(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new RadioButton(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new DataGridView(), new string[] { "DataSource: Object", "DataContext: Object", "Tag: Object" } },
        { new DateTimePicker(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new GroupBox(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new Label(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new ComboBox(), new string[] { "DataSource: Object", "DataContext: Object", "Tag: Object" } },
        { new ListBox(), new string[] { "DataSource: Object", "DataContext: Object", "Tag: Object" } },
        { new ListView(), new string[] { "DataContext: Object", "Tag: Object" } },
        {
            new MonthCalendar(), new string[]
            {
                "AnnuallyBoldedDates: DateTime[]",
                "BoldedDates: DateTime[]",
                "MonthlyBoldedDates: DateTime[]",
                "DataContext: Object",
                "Tag: Object"
            }
        },
        { new PictureBox(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new PrintPreviewControl(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new ProgressBar(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new ScrollableControl(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new HScrollBar(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new VScrollBar(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new Splitter(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new TabControl(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new TextBox(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new RichTextBox(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new MaskedTextBox(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new ToolStrip(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new TrackBar(), new string[] { "DataContext: Object", "Tag: Object" } },
        { new WebBrowser(), new string[] { "DataContext: Object", "Tag: Object" } },
    };
}
