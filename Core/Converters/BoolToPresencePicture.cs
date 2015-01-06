namespace MobileApp.Core.Converters
{
    #region

    using System;
    using System.Globalization;

    using Cirrious.CrossCore.Converters;

    #endregion

    /// <summary>
    /// Convert bool to image online/offline
    /// </summary>
    public class BoolToPresencePictureConverter : IMvxValueConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="value">
        /// par1
        /// </param>
        /// <param name="targetType">
        /// par2
        /// </param>
        /// <param name="parameter">
        /// par3
        /// </param>
        /// <param name="culture">
        /// par4
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? isOnline = (bool)value;
            if (isOnline.HasValue)
            {
                if (isOnline.Value)
                {
                    return "@android:drawable/presence_online";
                }

                return "@android:drawable/presence_invisible";
            }

            return null;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="value">
        /// par1
        /// </param>
        /// <param name="targetType">
        /// par2
        /// </param>
        /// <param name="parameter">
        /// par3
        /// </param>
        /// <param name="culture">
        /// par4
        /// </param>
        /// <returns>
        /// bool
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}