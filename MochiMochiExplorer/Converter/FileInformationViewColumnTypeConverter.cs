using MochiMochiExplorer.ViewModel.Wpf.FileInformation;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MochiMochiExplorer.Converter
{
    internal class FileInformationViewColumnTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var current = (FileInformationViewColumnType)value;
            var target = (FileInformationViewColumnType)parameter;

            return current.HasFlag(target) ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class FileInformationViewColumnTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var current = (FileInformationViewColumnType)value;
            var target = (FileInformationViewColumnType)parameter;

            return current.HasFlag(target);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
