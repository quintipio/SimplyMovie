﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SimplyMovieWin10.Converter
{
    /// <summary>
    /// Convertisseur d'un boolean en indication de Visible
    /// </summary>
    public class BoolVisibilityInvertConverter : IValueConverter
    {
        /// <summary>
        /// Boolean to Visible
        /// </summary>
        /// <param name="value">la donnée</param>
        /// <param name="targetType">targettype</param>
        /// <param name="parameter">parameter</param>
        /// <param name="language">Culture</param>
        /// <returns>la conversion</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var ret = ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
            return ret;
        }

        /// <summary>
        /// Visible to boolean
        /// </summary>
        /// <param name="value">la donnée</param>
        /// <param name="targetType">targettype</param>
        /// <param name="parameter">parameter</param>
        /// <param name="language">Culture</param>
        /// <returns>la conversion</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var ret = ((Visibility)value != Visibility.Visible);
            return ret;
        }
    }
}
