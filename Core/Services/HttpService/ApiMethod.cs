namespace MobileApp.Core.Services.HttpService
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    using Cirrious.CrossCore;

    using MobileApp.Core.Interfaces;
    using MobileApp.Core.Services.UtilityService.Logger;

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// The http provider.
    /// </summary>
    internal class ApiMethod
    {
        #region Fields

        private readonly string contentType;

        private readonly bool mediaRequest;

        private readonly string method;

        private readonly string uri;

        private readonly bool useComplexRequest;

        #endregion

        #region Constructors and Destructors

        public ApiMethod(
            string uri,
            MethodType method = MethodType.GET,
            bool useComplexRequest = false,
            bool mediaRequest = false,
            string contentType = "application/json")
            : this(uri, method.ToString(), useComplexRequest, mediaRequest, contentType)
        {
        }

        private ApiMethod(string uri, string method, bool useComplexRequest, bool mediaRequest, string contentType)
        {
            this.uri = uri;
            this.method = method;
            this.useComplexRequest = useComplexRequest;
            this.ParameterNames = new List<string>();
            this.mediaRequest = mediaRequest;
            this.contentType = contentType;

            this.GetAllParameters(uri);
        }

        #endregion

        private ISessionContainer Session
        {
            get
            {
                return Mvx.Resolve<ISessionContainer>();
            }
        }

        #region Public Properties

        /// <summary>
        /// Defines that response is simple binary stream
        /// </summary>
        public bool MediaRequest
        {
            get
            {
                return this.mediaRequest;
            }
        }

        /// <summary>
        /// Determines method
        /// </summary>
        public string Method
        {
            get
            {
                return this.method;
            }
        }

        /// <summary>
        /// List of Parameters
        /// </summary>
        public List<string> ParameterNames { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The request.
        /// </summary>
        /// <param name="success">
        /// The success.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void Request(Action<object> success = null, Action<Exception> error = null, IDictionary<string, object> parameters = null)
        {
            var url = this.Session.ServiceUrl + this.CreateUrlParameters(parameters);

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = this.Method;
            request.CookieContainer = Session.Cookies;
            request.ContentType = this.contentType;

            try
            {
                if (this.useComplexRequest)
                {
                    request.BeginGetRequestStream(result => WebResponces.ComplexRequest(success, error, parameters, request, result), request);
                }
                else
                {
                    if (this.MediaRequest)
                    {
                        request.BeginGetResponse(result => WebResponces.GetStream(success, error, request, result), null);
                    }
                    else
                    {
                        request.BeginGetResponse(result => WebResponces.Get(success, error, request, result), null);
                    }
                }
            }
            catch (Exception exception)
            {
                if (error != null)
                {
                    error(exception);
                    Mvx.Resolve<ILogger>().Error(exception, url);
                }
            }
        }

        /// <summary>
        /// The request.
        /// </summary>
        /// <param name="success">
        /// The success.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public virtual void Request<T>(Action<Responce<T>> success = null, Action<Exception> error = null, IDictionary<string, object> parameters = null)
        {
            var url = this.Session.ServiceUrl + this.CreateUrlParameters(parameters);

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = this.Method;
            request.CookieContainer = Session.Cookies;
            request.ContentType = this.contentType;

            try
            {
                if (this.useComplexRequest)
                {
                    request.BeginGetRequestStream(
                        a =>
                            {
                                // End the operation
                                var postStream = request.EndGetRequestStream(a);
                                var data = JsonConvert.SerializeObject(parameters);
                                var postData = Encoding.UTF8.GetBytes(data);
                                postStream.WriteAsync(postData, 0, postData.Length).ContinueWith(
                                    o =>
                                        {
                                            postStream.Dispose();

                                            try
                                            {
                                                request.BeginGetResponse(result => WebResponces.Get(success, error, request, result), null);
                                            }
                                            catch (Exception exception)
                                            {
                                                if (error != null)
                                                {
                                                    error(exception);
                                                    Mvx.Resolve<ILogger>().Error(exception, parameters);
                                                }
                                            }
                                        });
                            },
                        request);
                }
                else
                {
                    if (this.MediaRequest)
                    {
                        request.BeginGetResponse(result => WebResponces.GetStream((Action<Responce<Stream>>)success, error, request, result), null);
                    }
                    else
                    {
                        Session.Cookies = request.CookieContainer;
                        request.BeginGetResponse(result => WebResponces.Get(success, error, request, result), null);
                    }
                }
            }
            catch (Exception exception)
            {
                if (error != null)
                {
                    error(exception);
                    Mvx.Resolve<ILogger>().Error(exception, url);
                }
            }
        }

        #endregion

        #region Methods

        private void GetAllParameters(string pathString)
        {
            var temp = pathString;
            while (temp.Contains("{"))
            {
                var start = temp.IndexOf('{') + 1;
                var length = temp.IndexOf('}') - start;

                this.ParameterNames.Add(temp.Substring(start, length).ToUpper());
                temp = temp.Remove(start - 1, length + 2);
            }
        }

        private string CreateUrlParameters(IDictionary<string, object> parameters)
        {
            return this.ParameterNames == null
                       ? this.uri
                       : this.ParameterNames.Aggregate(this.uri, (current, param) => current.Replace("{" + param + "}", parameters[param].ToString()));
        }

        #endregion
    }
}