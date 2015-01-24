namespace MvvmCross.Helper.Core.Converters
{
    using System.Collections.Generic;
    using System.Linq;

    using Cirrious.CrossCore;
    using Cirrious.MvvmCross;
    using Cirrious.MvvmCross.Platform;

    /// <summary>
    /// Convers this object to dictionary of property and values
    /// </summary>
    public static class DynamicExtentions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Check and convert object to dynamic properties dictionary
        /// </summary>
        /// <param name="input">suspicious object</param>
        /// <returns>collection of properties</returns>
        public static IDictionary<string, object> ToAbstractPropertyDictionary(this object input)
        {
            if (input == null)
            {
                return new Dictionary<string, object>();
            }

            var objects = input as IDictionary<string, object>;
            if (objects != null)
            {
                return objects;
            }

            var propertyInfos = from property in input.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                                where property.CanRead
                                select new { CanSerialize = MvxSingletonCache.Instance.Parser.TypeSupported(property.PropertyType), Property = property };

            var dictionary = new Dictionary<string, object>();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.CanSerialize)
                {
                    dictionary[propertyInfo.Property.Name] = input.GetPropertyValueAsString(propertyInfo.Property);
                }
                else
                {
                    Mvx.Trace(
                        "Skipping serialization of property {0} - don't know how to serialize type {1} - some answers on http://stackoverflow.com/questions/16524236/custom-types-in-navigation-parameters-in-v3",
                        propertyInfo.Property.Name,
                        propertyInfo.Property.PropertyType.Name);
                }
            }
            return dictionary;
        }

        #endregion
    }
}