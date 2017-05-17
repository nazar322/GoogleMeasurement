using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Google.Data.Measurement.Helpers
{
    internal static class HttpHelper
    {
        #region CallbackToken
        private class CallbackToken
        {
            internal HttpWebRequest Request;
            internal ManualResetEvent WaitHandle;
        }
        #endregion

        #region PostData
        private sealed class PostData : CallbackToken
        {
            internal string Body;
            internal Encoding Encoding;
        }
        #endregion

        #region SendPostRequest
        internal static void SendPostRequest(string url, string body, Encoding encoding)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var postData = new PostData { Request = request, Body = body, Encoding = encoding, WaitHandle = new ManualResetEvent(false) };

            request.Method = "POST";

            request.BeginGetRequestStream(BeginGetRequestStreamCallback, postData);

            postData.WaitHandle.WaitOne();
        }
        #endregion

        #region SendPostRequestAsync
        internal static void SendPostRequestAsync(string url, string body, Encoding encoding)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var postData = new PostData { Request = request, Body = body, Encoding = encoding };

            request.Method = "POST";

            request.BeginGetRequestStream(BeginGetRequestStreamCallback, postData);
        }
        #endregion

        #region SendGetRequest
        internal static void SendGetRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var callbackToken = new CallbackToken { Request = request, WaitHandle = new ManualResetEvent(false) };

            request.BeginGetResponse(BeginGetResponseCallback, callbackToken);

            callbackToken.WaitHandle.WaitOne();
        }
        #endregion

        #region SendGetRequestAsync
        internal static void SendGetRequestAsync(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var callbackToken = new CallbackToken { Request = request };

            request.BeginGetResponse(BeginGetResponseCallback, callbackToken);
        }
        #endregion

        #region BeginGetRequestStreamCallback
        private static void BeginGetRequestStreamCallback(IAsyncResult ar)
        {
            var postData = (PostData)ar.AsyncState;
            var status = true;

            Stream requestStream = null;

            try
            {
                requestStream = postData.Request.EndGetRequestStream(ar);

                if (requestStream != null)
                {
                    using (var streamWriter = new StreamWriter(requestStream, postData.Encoding))
                    {
                        requestStream = null;

                        streamWriter.Write(postData.Body);
                    }
                }
            }
            catch (WebException)
            {
                status = false;
                // Swallow WebException issues
                if (postData.WaitHandle != null)
                    postData.WaitHandle.Set();
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Dispose();
            }

            if (status)
                postData.Request.BeginGetResponse(BeginGetResponseCallback, postData);
        }
        #endregion

        #region BeginGetResponseCallback
        private static void BeginGetResponseCallback(IAsyncResult ar)
        {
            var callbackToken = (CallbackToken)ar.AsyncState;

            try
            {
                using (callbackToken.Request.EndGetResponse(ar))
                {
                }
            }
            catch { /* Ignore */ }
            finally
            {
                if (callbackToken.WaitHandle != null)
                    callbackToken.WaitHandle.Set();
            }
        }
        #endregion
    }
}
