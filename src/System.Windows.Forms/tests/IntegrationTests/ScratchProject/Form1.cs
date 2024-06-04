// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;

namespace ScratchProject;

// As we can't currently design in VS in the runtime solution, mark as "Default" so this opens in code view by default.
[DesignerCategory("Default")]
public partial class Form1 : Form
{
    public Form1()
    {
/*        Stream stream = new MemoryStream();
        JsonSerializer.Serialize(stream, new CustomDataObject() { Name = "loni", Location = new()});
        stream.Position = 0;
        var rehydrate = JsonSerializer.Deserialize(stream, typeof(CustomDataObject));
        Clipboard.SetDataObject("test");
        object? test = Clipboard.GetDataObject();*/
        InitializeComponent();
    }

    private void copyButton_Click(object sender, EventArgs e)
    {
        WeatherForecast weatherForecast = new WeatherForecast
        {
            Date = DateTimeOffset.Now,
            TemperatureCelsius = 25,
            Summary = "Hot"
        };

        Clipboard.SetData("weather", weatherForecast);
    }

    private void pasteButton_Click(object sender, EventArgs e)
    {
        if (Clipboard.ContainsData("weather"))
        {
            WeatherForecast weather = (WeatherForecast)Clipboard.GetData("weather");
        }
    }

    private void beginDrag_MouseDown(object sender, MouseEventArgs e)
    {
        WeatherForecast weatherForecast = new WeatherForecast
        {
            Date = DateTimeOffset.Now,
            TemperatureCelsius = 25,
            Summary = "Hot"
        };

        //beingDrag.DoDragDropAsJson(weatherForecast, DragDropEffects.Copy);
        beingDrag.DoDragDrop<WeatherForecast>(weatherForecast, DragDropEffects.Copy);
    }

    private void Form1_DragOver(object sender, DragEventArgs e)
    {
        e.Effect = DragDropEffects.Copy;
    }

    private void Form1_DragDrop(object sender, DragEventArgs e)
    {
        object test = e.Data.GetData(typeof(WeatherForecast));
    }

    /*    private class CustomDataObject : ComTypes.IDataObject
        {
            public PointF Location { get; set; }
            public string Name { get; set; } = "Test";
            [DllImport("shell32.dll")]
            public static extern int SHCreateStdEnumFmtEtc(uint cfmt, ComTypes.FORMATETC[] afmt, out ComTypes.IEnumFORMATETC ppenumFormatEtc);

            int ComTypes.IDataObject.DAdvise(ref ComTypes.FORMATETC pFormatetc, ComTypes.ADVF advf, ComTypes.IAdviseSink adviseSink, out int connection) => throw new NotImplementedException();
            void ComTypes.IDataObject.DUnadvise(int connection) => throw new NotImplementedException();
            int ComTypes.IDataObject.EnumDAdvise(out ComTypes.IEnumSTATDATA enumAdvise) => throw new NotImplementedException();
            ComTypes.IEnumFORMATETC ComTypes.IDataObject.EnumFormatEtc(ComTypes.DATADIR direction)
            {
                if (direction == ComTypes.DATADIR.DATADIR_GET)
                {
                    // Create enumerator and return it
                    ComTypes.IEnumFORMATETC enumerator;
                    if (SHCreateStdEnumFmtEtc(0, [], out enumerator) == 0)
                    {
                        return enumerator;
                    }
                }

                throw new NotImplementedException();
            }

            int ComTypes.IDataObject.GetCanonicalFormatEtc(ref ComTypes.FORMATETC formatIn, out ComTypes.FORMATETC formatOut) => throw new NotImplementedException();
    *//*        object IDataObject.GetData(string format, bool autoConvert) => format == "Foo" ? "Bar" : null;
            object IDataObject.GetData(string format) => format == "Foo" ? "Bar" : null;
            object IDataObject.GetData(Type format) => null;*//*
            void ComTypes.IDataObject.GetData(ref ComTypes.FORMATETC format, out ComTypes.STGMEDIUM medium) => throw new NotImplementedException();
            void ComTypes.IDataObject.GetDataHere(ref ComTypes.FORMATETC format, ref ComTypes.STGMEDIUM medium) => throw new NotImplementedException();
    *//*        bool IDataObject.GetDataPresent(string format, bool autoConvert) => format == "Foo";
            bool IDataObject.GetDataPresent(string format) => format == "Foo";
            bool IDataObject.GetDataPresent(Type format) => false;
            string[] IDataObject.GetFormats(bool autoConvert) => ["Foo"];
            string[] IDataObject.GetFormats() => ["Foo"];*//*
            int ComTypes.IDataObject.QueryGetData(ref ComTypes.FORMATETC format) => throw new NotImplementedException();
    *//*        void IDataObject.SetData(string format, bool autoConvert, object data) => throw new NotImplementedException();
            void IDataObject.SetData(string format, object data) => throw new NotImplementedException();
            void IDataObject.SetData(Type format, object data) => throw new NotImplementedException();
            void IDataObject.SetData(object data) => throw new NotImplementedException();*//*
            void ComTypes.IDataObject.SetData(ref ComTypes.FORMATETC formatIn, ref ComTypes.STGMEDIUM medium, bool release) => throw new NotImplementedException();
        }*/

}

[Serializable]
public class WeatherForecast
{
    public DateTimeOffset Date { get; set; }
    public int TemperatureCelsius { get; set; }
    public string? Summary { get; set; }
}

