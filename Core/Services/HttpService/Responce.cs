namespace MobileApp.Core.Services.HttpService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Services.UtilityService.Logger;

    using Newtonsoft.Json;

    /// <summary>
    /// Interface of Response to client on abstract request
    /// </summary>
    /// <typeparam name="T">type of responding data</typeparam>
    public interface IResponce<T> : IResponce
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the data
        /// </summary>
        [JsonProperty("data", Required = Required.AllowNull)]
        T Data { get; set; }

        /// <summary>
        /// Type for
        /// </summary>
        [JsonProperty("type")]
        Type Type { get; }

        #endregion
    }

    public class Responce : IResponce<Object>
    {
        public static Responce Ok = new Responce { Result = true };

        public static Responce Bad = new Responce { Result = false };

        public bool Result { get; set; }

        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public Type Type { get; private set; }
    }

    /// <summary>
    /// Response to client on abstract request
    /// </summary>
    public class Responce<T> : IResponce<T>
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the user data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Error Code
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Is successfull
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// Type of Responding object
        /// </summary>
        public Type Type
        {
            get
            {
                return this.Data.GetType();
            }
        }

        /// <summary>
        /// Process responce with list
        /// </summary>
        /// <typeparam name="T">type of responce's content</typeparam>
        /// <param name="obj">responce</param>
        /// <param name="logger">where to write</param>
        /// <param name="sender">from whom</param>
        /// <returns></returns>
        public static List<T> ProcessListResponce(object obj, ILogger logger, object sender)
        {
            var data = JsonConvert.DeserializeObject<Responce>(obj.ToString());
            if (data == null)
            {
                logger.Debug(string.Format("{0} - Failed to deserialize {1} to VmcResponce", sender.GetType().Name), sender);
                return null;
            }

            if (!data.Result)
            {
                logger.Debug(string.Format("{0} - Request result - {2} : {1} ", sender.GetType().Name, data.Message, data.ErrorCode), sender);
            }
            else
            {
                logger.Debug(string.Format("{0} - Request result - OK", sender.GetType().Name), sender);
                try
                {
                    if (data.Data != null)
                    {
                        var jData = data.Data.ToString();
                        var list = JsonConvert.DeserializeObject<IEnumerable<T>>(jData, new JsonSerializerSettings()).ToList();
                        logger.Debug(string.Format("{0} - Collection serialized {1}", sender.GetType().Name, jData), sender);

                        return list;
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e, sender);
                }
            }

            return null;
        }

        #endregion
    }
}
