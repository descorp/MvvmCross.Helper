namespace MvvmCross.Helper.Core.Converters
{
    #region

    using System.Globalization;

    using Cirrious.CrossCore.UI;
    using Cirrious.MvvmCross.Plugins.Color;

    #endregion

    /// <summary>
    /// Bool to Color Converter
    /// </summary>
    public class BoolToColorConverter : MvxColorValueConverter
    {
        #region Methods

        /// <summary>
        /// Converte to picture url
        /// </summary>
        /// <param name="value">
        /// bool
        /// </param>
        /// <param name="parameter">
        /// somthing
        /// </param>
        /// <param name="culture">
        /// azaza
        /// </param>
        /// <returns>
        /// string
        /// </returns>
        protected override MvxColor Convert(object value, object parameter, CultureInfo culture)
        {
            bool isVisible;

            if (bool.TryParse(value.ToString(), out isVisible))
            {
                if (isVisible)
                {
                    return new MvxColor(0xFF, 0xff, 0xff);
                }
                return new MvxColor(0x79, 0x98, 0xb0);
                
            }

            return null;
        }

        #endregion
    }
}