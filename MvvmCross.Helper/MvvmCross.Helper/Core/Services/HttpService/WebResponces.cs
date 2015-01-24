namespace MvvmCross.Helper.Core.Services.HttpService
{
    #region

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    using Cirrious.CrossCore;

    using MvvmCross.Helper.Core.Interfaces;
    using MvvmCross.Helper.Core.Properties;
    using MvvmCross.Helper.Core.Services.UtilityService.Logger;

    using Newtonsoft.Json;

    #endregion

    /// <summary>
    /// The templates for responses.
    /// </summary>
    public class WebResponces
    {

        #region Public Methods and Operators

        /// <summary>
        /// Get generic response
        /// </summary>
        /// <param name="success">
        /// The success.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="result">
        /// The Result.
        /// </param>
        public static void Get(Action<object> success, Action<Exception> error, HttpWebRequest request, IAsyncResult result)
        {
            try
            {
                var state = result.AsyncState as HttpWebRequest;

                if (state != null)
                {
                    Mvx.Resolve<ISessionContainer>().Cookies = state.CookieContainer;
                }

                var response = request.EndGetResponse(result);

                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    success(text);
                }
            }
            catch (WebException exc)
            {
                if (exc.Message.Contains("The remote server returned an error: (401) Unauthorized."))
                {
                    success(new Responce { Result = false, Data = null, Message = exc.Message, ErrorCode = 401 });
                }
                else if (error != null)
                {
                    error(exc.Message.Contains("Connection timed out") ? new Exception(Resources.ServerNotAvailable) : new Exception(Resources.NetworkError));
                }
            }
            catch (Exception exception)
            {
                if (error != null)
                {
                    error(exception);
                    Mvx.Resolve<ILogger>().Error(exception);
                }
            }
        }

        /// <summary>
        /// Get generic response
        /// </summary>
        /// <param name="success">
        /// The success.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="result">
        /// The Result.
        /// </param>
        public static void ComplexRequest(Action<object> success, Action<Exception> error, IDictionary<string, object> parameters, HttpWebRequest request, IAsyncResult result)
        {
            // End the operation
            var postStream = request.EndGetRequestStream(result);
            var text =  JsonConvert.SerializeObject(parameters.ContainsKey("BODY") ? parameters["BODY"] : parameters.First()) ;
            var postData = Encoding.UTF8.GetBytes(text);

            postStream.WriteAsync(postData, 0, postData.Length);
            
            postStream.FlushAsync().ContinueWith(
                o =>
                {
                    try
                    {
                        request.BeginGetResponse(r => Get(success, error, request, r), postStream);
                    }
                    catch (Exception exception)
                    {
                        if (error != null)
                        {
                            error(exception);
                            Mvx.Resolve<ILogger>().Error(exception, parameters);
                        }
                    }
                    finally
                    {
                        //postStream.Dispose();
                    }
                });
        }

        /// <summary>
        /// The media response.
        /// </summary>
        /// <param name="success">
        /// The success.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="result">
        /// The Result.
        /// </param>
        public static void GetStream(Action<object> success, Action<Exception> error, HttpWebRequest request, IAsyncResult result)
        {
            try
            {
                var httpResponse = (HttpWebResponse)request.EndGetResponse(result);
                var stream = new MemoryStream();

                var responseStream = httpResponse.GetResponseStream();
                if (responseStream != null)
                {
                    responseStream.CopyTo(stream);
                }

                success(stream);
            }
            catch (WebException exc)
            {
                if (exc.Message.Contains("The remote server returned an error: (401) Unauthorized."))
                {
                    success(new Responce { Result = false, Data = null, Message = exc.Message, ErrorCode = 401 });
                }
                else if (error != null)
                {
                    error(exc.Message.Contains("Connection timed out") ? new Exception(Resources.ServerNotAvailable) : new Exception(Resources.NetworkError));
                }
            }
            catch (Exception exception)
            {
                if (error != null)
                {
                    error(exception);
                }
            }
        }



        /// <summary>
        /// Get generic response
        /// </summary>
        /// <param name="success">
        /// The success.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="result">
        /// The Result.
        /// </param>
        public static void Get<T>(Action<Responce<T>> success, Action<Exception> error, HttpWebRequest request, IAsyncResult result)
        {
            try
            {
                var response = request.EndGetResponse(result);

                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    var responce = JsonConvert.DeserializeObject<Responce<T>>(text);
                    success(responce);
                }
            }
            catch (WebException exc)
            {
                if (exc.Message.Contains("The remote server returned an error: (401) Unauthorized."))
                {
                    success(new Responce<T> { Result = false, Data = default(T), Message = exc.Message, ErrorCode = 401 });
                }
                else if (error != null)
                {
                    error(exc.Message.Contains("Connection timed out") ? new Exception(Resources.ServerNotAvailable) : new Exception(Resources.NetworkError));
                }
            }
            catch (Exception exception)
            {
                if (error != null)
                {
                    error(exception);
                }
            }
        }

        /// <summary>
        /// The media response.
        /// </summary>
        /// <param name="success">
        /// The success.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="result">
        /// The Result.
        /// </param>
        public static void GetStream(Action<Responce<Stream>> success, Action<Exception> error, HttpWebRequest request, IAsyncResult result)
        {
            try
            {
                var httpResponse = (HttpWebResponse)request.EndGetResponse(result);
                var stream = new MemoryStream();

                var responseStream = httpResponse.GetResponseStream();
                if (responseStream != null)
                {
                    responseStream.CopyTo(stream);
                    responseStream.Dispose();
                }

                success(new Responce<Stream>{Result = true, Data = stream});
            }
            catch (WebException exc)
            {
                if (exc.Message.Contains("The remote server returned an error: (401) Unauthorized."))
                {
                    success(new Responce<Stream> { Result = false, Data = null, Message = exc.Message, ErrorCode = 401 });
                }
                else if (error != null)
                {
                    error(exc.Message.Contains("Connection timed out") ? new Exception(Resources.ServerNotAvailable) : new Exception(Resources.NetworkError));
                }
            }
            catch (Exception exception)
            {
                if (error != null)
                {
                    error(exception);
                }
            }
        }

        #endregion
    }
}