namespace MobileApp.Core.Converters
{
    #region

    using System;
    using System.Globalization;

    using Cirrious.CrossCore.Converters;
    using Cirrious.CrossCore.UI;

    #endregion

    /// <summary>
    /// The bool to visibility.
    /// </summary>
    public class BoolToVisibility : MvxValueConverter<bool, MvxVisibility>
    {
        #region Methods

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <returns>
        /// The <see cref="MvxVisibility" />.
        /// </returns>
        protected override MvxVisibility Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? MvxVisibility.Visible : MvxVisibility.Collapsed;
        }

        #endregion
    }
}