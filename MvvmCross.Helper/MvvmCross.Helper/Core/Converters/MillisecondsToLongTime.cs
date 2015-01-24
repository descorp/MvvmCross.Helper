namespace MvvmCross.Helper.Core.Converters
{
    using System;
    using System.Globalization;

    using Cirrious.CrossCore.Converters;

    /// <summary>
    /// The timestamp to date.
    /// </summary>
    public class MillisecondsToLongTime : MvxValueConverter<double, string>
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
        /// The <see cref="string" />.
        /// </returns>
        protected override string Convert(double value, Type targetType, object parameter, CultureInfo culture)
        {
            var secondsTotal = (int)Math.Floor(value) / 1000;
            var milliseconds = value - secondsTotal * 1000;
            var minutes = secondsTotal / 60;
            var seconds = secondsTotal - minutes * 60;
            return string.Format(
                "{0}:{1}.{2}",
                ((minutes < 10) ? "0" : string.Empty) + minutes,
                ((seconds < 10) ? "0" : string.Empty) + seconds,
                milliseconds);

        }

        #endregion
    }
}
