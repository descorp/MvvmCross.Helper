namespace MobileApp.Core.Services.HttpService
{
    #region

    using System;
    using System.Collections.Generic;

    using Cirrious.CrossCore;

    using MobileApp.Core.Converters;
    using MobileApp.Core.Models;
    using MobileApp.Core.Properties;
    using MobileApp.Core.Services.UtilityService;
    using MobileApp.Core.Services.UtilityService.Logger;

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// The Mediation Server's API provider.
    /// </summary>
    public class ApiInteraction
    {
        #region Static Fields

        private static readonly object IsBusy = new object();

        #endregion

        #region Fields

        private readonly Action<Exception> externalError;

        private readonly IDictionary<string, object> externalParams;

        private readonly Action<object> externalSuccess;

        private readonly ApiMethodType externalType;

        private readonly DateTime timer = DateTime.Now;

        #endregion

        #region Constructors and Destructors

        private ApiInteraction(ApiMethodType type, Action<object> success = null, Action<Exception> error = null, IDictionary<string, object> parametrs = null)
        {
            this.externalSuccess = o =>
                {
                    Mvx.Resolve<ILogger>().Debug(string.Format("API {0}: Ok; duration: {2} param: {1}", type, parametrs, (DateTime.Now - timer).TotalMilliseconds ), this);
                    success(o);
                };

            this.externalError = o =>
                {
                    Mvx.Resolve<ILogger>().Debug(string.Format("API {0}: Error; duration: {2} param: {1}; Error: {3}", type, parametrs, (DateTime.Now - timer).TotalMilliseconds, o), this);
                    error(o);
                };

            this.externalParams = parametrs;
            this.externalType = type;
        }

        #endregion

        #region Public Properties

        public static CredentialsModel Authentication { private get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Create request to server
        /// </summary>
        /// <param name="type">
        /// Type of operation
        /// </param>
        /// <param name="success">
        /// Success callback
        /// </param>
        /// <param name="error">
        /// InternalError callback
        /// </param>
        /// <param name="parameters">
        /// Some parameters needs for success <see cref="ApiInteraction" />
        /// </param>
        /// <returns>
        /// The <see cref="ApiInteraction" />.
        /// </returns>
        public static ApiInteraction Create(ApiMethodType type, IDictionary<string, object> parameters, Action<object> success = null, Action<Exception> error = null)
        {
            var item = new ApiInteraction(type, success, error, parameters);

            Request(error, item, parameters);

            return item;
        }

        /// <summary>
        /// Create request to server
        /// </summary>
        /// <param name="type">
        /// Type of operation
        /// </param>
        /// <param name="success">
        /// Success callback
        /// </param>
        /// <param name="error">
        /// InternalError callback
        /// </param>
        /// <param name="data"></param>
        /// <returns>
        /// The <see cref="ApiInteraction" />.
        /// </returns>
        public static ApiInteraction Create(ApiMethodType type, Action<object> success = null, Action<Exception> error = null, object data = null)
        {
            var parameters = data.ToAbstractPropertyDictionary();
            var item = new ApiInteraction(type, success, error, parameters);

            Request(error, item, parameters);

            return item;
        }

        public static ApiInteraction Get(Action<object> success, Action<Exception> error = null, Type type = null)
        {
            return GetApiService(success, error, type);
        }

        #endregion

        #region Methods

        private static ApiInteraction GetApiService(Action<object> success, Action<Exception> error, Type type, IDictionary<string, object> parameters = null)
        {
            ApiInteraction item;
            switch (type.Name)
            {
                case "Contact":
                    item = new ApiInteraction(ApiMethodType.GetContacts, success, error);
                    break;
                case "Device":
                    item = parameters != null
                               ? new ApiInteraction(ApiMethodType.UpdateDeviceState, success, error, parameters)
                               : new ApiInteraction(ApiMethodType.GetDevices, success, error);
                    break;
                case "Queue":
                    item = new ApiInteraction(ApiMethodType.GetQueues, success, error);
                    break;
                case "Number":
                    item = new ApiInteraction(ApiMethodType.GetNumbers, success, error);
                    break;
                case "Voicemail":
                    item = new ApiInteraction(ApiMethodType.GetVoicemails, success, error);
                    break;
                default:
                    return null;
            }

            Request(error, item);

            return item;
        }

        private static void Request(Action<Exception> error, ApiInteraction item, IDictionary<string, object> parameters = null)
        {
            lock (IsBusy)
            {
                var apiMethod = ApiMethodsCollections.GetMethod(item.externalType);
                apiMethod.Request(apiMethod.MediaRequest ? item.externalSuccess : item.InternalSuccess, error, parameters);
            }
        }

        private void InternalSuccess(object obj)
        {
            try
            {
                var responce = obj as Responce ?? JsonConvert.DeserializeObject<Responce>(obj.ToString());
                if (responce.Result)
                {
                    this.externalSuccess(obj);
                }
                else
                {
                    if (responce.ErrorCode == 401 && Authentication.IsValid())
                    {
                        Mvx.Resolve<ILogger>().Debug(string.Format("{0} - {1} - 401 Session Timeout. Reconnecting... ", this.GetType().Name, this.externalType), this);

                        ApiMethodsCollections.GetMethod(ApiMethodType.GetAuth).Request(
                            a =>
                                {
                                    Responce resp;
                                    try
                                    {
                                        resp = JsonConvert.DeserializeObject<Responce>(a.ToString());

                                    }
                                    catch (Exception e)
                                    {
                                        Mvx.Resolve<ILogger>().Error(e, this);
                                        return;
                                    }

                                    if (resp.Result)
                                    {
                                        ApiMethodsCollections.GetMethod(this.externalType).Request(this.InternalSuccess, this.externalError, this.externalParams);
                                    }
                                    else
                                    {
                                        Mvx.Resolve<ILogger>().Debug(string.Format("Reconnection unsuccessful"), this);
                                        Mvx.Resolve<INavigateMyselfService>().Breakout("Error authorization");
                                    }
                                },
                            exception => Mvx.Resolve<INavigateMyselfService>().Breakout("Reconnection unsuccessful"),
                            new Dictionary<string, object> { { "INSTANCE", Authentication.Instance }, { "PIN", Authentication.Pin }, { "USERID", Authentication.UserId }, { "VERSION", Resources.BuildVersion } });
                    }
                    else
                    {
                        this.externalSuccess(responce);
                    }
                }
            }
            catch (Exception exc)
            {
                this.externalError(exc);
            }
        }

        #endregion
    }
}