using DevExpress.Xpf.Core;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace CharityManager.UI.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IDToTitle : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(parameter?.ToString()))
                return "Parameter is null";
            if (value == null)
                return string.Empty;

            if (int.TryParse(value.ToString(), out int id))
            {
                if (id == 0) return "";

                switch (parameter.ToString())
                {
                    case nameof(IDToTitleEntity.Entity):
                        return AppConfigs.All.FirstOrDefault(e => e.ID == id)?.Title ?? $"Invalid {parameter} ID";
                    case string str when str.StartsWith("$"):
                        var list = (AppUIManager.Application.TryFindResource($"List.{parameter.ToString().Replace("$","")}") as ArrayList).ToArray();
                        var result = list.OfType<DictionaryEntry>().FirstOrDefault(e => e.Key.ToString() == id.ToString()).Value;
                        return result;
                    case nameof(IDToTitleEntity.Introducer):
                        return AppConfigs.Introducers.FirstOrDefault(i => i.Key == id).Value ?? $"شناسه {id} یافت نشد";
                }
            }
            return $"Invalid value '{value}'";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public class Money : IValueConverter
    {
        public bool Unit { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out int num))
                return num.ToString("N0") + (Unit ? " ریال " : "");
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(long), typeof(string))]
    public class LongToSize : IValueConverter
    {
        private static string[] NAMES = new string[] { "", "K", "M", "G" };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (long.TryParse(value?.ToString(), out long size))
            {
                int count = 0;
                while (size / 1024 > 0)
                {
                    size /= 1024;
                    count++;
                }
                return $"{size:N0} {NAMES[count]}B";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ValueConversion(typeof(Color), typeof(Color))]
    public class ColorBrightnessConverter : IValueConverter
    {
        public double Brightness { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color baseColor)
                return baseColor.ChangeColorBrightness(Brightness);
            if (value is Brush baseBrush)
                return baseBrush.SetBrightness(Brightness);
            throw new ArgumentException($"{value} is not a valid color or brush", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
