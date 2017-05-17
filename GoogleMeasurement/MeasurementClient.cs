using System;
using System.Text;
using Google.Data.Measurement.Helpers;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Sends hits to Google Analytics.
    /// </summary>
    public sealed class MeasurementClient
    {
        #region Constants
        private const string ApiEndpointUrl = "http://www.google-analytics.com/collect";
        private const string ApiEndpointUrlSsl = "https://ssl.google-analytics.com/collect";

        #region Validation
        private const int MaxLengthBytesPostBody = 8192;
        private const int MaxLengthBytesUrl = 2000;
        private const int MaxLengthBytesUserLanguage = 20;
        #endregion

        #region Parameters
        private const string VersionParameter = "v=1";
        private const string TrackingIdParameter = "&tid=";
        private const string ClientIdParameter = "&cid=";
        private const string UserAgentParameter = "&ua=";
        private const string UserLanguageParameter = "&ul=";
        #endregion
        #endregion

        #region Constructors
        private MeasurementClient()
        {
            Method = "POST";
        }

        private MeasurementClient(string trackingId)
            : this()
        {
            TrackingId = trackingId;
        }

        public MeasurementClient(string trackingId, Guid clientId)
            : this(trackingId)
        {
            ClientId = clientId;
        }
        #endregion

        #region Properties
        #region TrackingId
        /// <summary>
        /// The tracking ID / web property ID. The format is UA-XXXX-Y. All collected data is associated by this ID.
        /// </summary>
        public string TrackingId { get; private set; }
        #endregion

        #region ClientId
        /// <summary>
        /// This anonymously identifies a particular user, device, or browser instance.
        /// <para>If not set a unique id would be auto-generated upon each hit.</para>
        /// </summary>
        public Guid? ClientId { get; set; }
        #endregion

        #region Method
        private string _method;

        /// <summary>
        /// Method for the request.
        /// </summary>
        public string Method
        {
            get { return _method; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                value = value.ToUpperInvariant();

                switch (value)
                {
                    case "GET":
                    case "POST":
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("value", "Only \"POST\" or \"GET\" are valid values");
                }

                _method = value;
            }
        }
        #endregion

        #region UseSsl
        /// <summary>
        /// If true hits would be sent to HTTPS endpoint.
        /// </summary>
        public bool UseSsl { get; set; }
        #endregion

        #region UserAgent
        private string _userAgent;
        private string _userAgentEncoded;

        /// <summary>
        /// The User Agent of the browser.
        /// <para>This parameter is optional.</para>
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string UserAgent
        {
            get { return _userAgent; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _userAgent = _userAgentEncoded = value;
                }
                else
                {
                    _userAgentEncoded = value.UrlEncode();
                    _userAgent = value;
                }
            }
        }
        #endregion

        #region ThrowExceptions
        /// <summary>
        /// Throw exceptions on payload validation errors.
        /// </summary>
        public bool ThrowExceptions
        {
            get { return PayloadData.ThrowExceptions; }
            set { PayloadData.ThrowExceptions = value; }
        }
        #endregion

        #region UserLanguage
        private string _userLanguage;

        /// <summary>
        /// Specifies the language.
        /// </summary>
        public string UserLanguage
        {
            get { return _userLanguage; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _userLanguage = value;
                }
                else
                {
                    if (PayloadData.ThrowExceptions && value.Length > MaxLengthBytesUserLanguage)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"UserLanguage\" property should not exceed of {0} bytes", MaxLengthBytesUserLanguage));
                    }

                    _userLanguage = value;
                }
            }
        }
        #endregion
        #endregion

        #region Methods
        #region Hits
        #region PageView
        /// <summary>
        /// Send 'pageview' hit.
        /// </summary>
        /// <param name="pageViewData">The pageview payload data to be sent.</param>
        public void PageView(PageViewData pageViewData)
        {
            SendHit(HitType.PageView, pageViewData);
        }

        /// <summary>
        /// Send 'pageview' hit.
        /// </summary>
        public void PageView(string documentHostname, string documentPath, string documentTitle)
        {
            var pageViewData = new PageViewData
            {
                DocumentHostName = documentHostname,
                DocumentPath = documentPath,
                DocumentTitle = documentTitle
            };

            PageView(pageViewData);
        }

        /// <summary>
        /// Send 'pageview' hit.
        /// </summary>
        public void PageView(string documentLocation, string documentTitle)
        {
            var pageViewData = new PageViewData
            {
                DocumentLocation = documentLocation,
                DocumentTitle = documentTitle
            };

            PageView(pageViewData);
        }
        #endregion

        #region PageViewAsync
        /// <summary>
        /// Send 'pageview' hit asynchronously.
        /// </summary>
        /// <param name="pageViewData">The pageview payload data to be sent.</param>
        public void PageViewAsync(PageViewData pageViewData)
        {
            SendHitAsync(HitType.PageView, pageViewData);
        }

        /// <summary>
        /// Send 'pageview' hit asynchronously.
        /// </summary>
        public void PageViewAsync(string documentHostname, string documentPath, string documentTitle)
        {
            var pageViewData = new PageViewData
            {
                DocumentHostName = documentHostname,
                DocumentPath = documentPath,
                DocumentTitle = documentTitle
            };

            PageViewAsync(pageViewData);
        }

        /// <summary>
        /// Send 'pageview' hit asynchronously.
        /// </summary>
        public void PageViewAsync(string documentLocation, string documentTitle)
        {
            var pageViewData = new PageViewData
            {
                DocumentLocation = documentLocation,
                DocumentTitle = documentTitle
            };

            PageViewAsync(pageViewData);
        }
        #endregion

        #region ScreenView
        /// <summary>
        /// Send 'screenview' hit.
        /// </summary>
        /// <param name="screenViewData">The screenview payload data to be sent.</param>
        public void ScreenView(ScreenViewData screenViewData)
        {
            SendHit(HitType.ScreenView, screenViewData);
        }

        /// <summary>
        /// Send 'screenview' hit.
        /// </summary>
        public void ScreenView(string screenName, string applicationName, string applicationVersion)
        {
            var screenViewData = new ScreenViewData
            {
                ScreenName = screenName,
                ApplicationName = applicationName,
                ApplicationVersion = applicationVersion
            };

            ScreenView(screenViewData);
        }
        #endregion

        #region ScreenViewAsync
        /// <summary>
        /// Send 'screenview' hit asynchronously.
        /// </summary>
        /// <param name="screenViewData">The screenview payload data to be sent.</param>
        public void ScreenViewAsync(ScreenViewData screenViewData)
        {
            SendHitAsync(HitType.ScreenView, screenViewData);
        }

        /// <summary>
        /// Send 'screenview' hit asynchronously.
        /// </summary>
        public void ScreenViewAsync(string screenName, string applicationName, string applicationVersion)
        {
            var screenViewData = new ScreenViewData
            {
                ScreenName = screenName,
                ApplicationName = applicationName,
                ApplicationVersion = applicationVersion
            };

            ScreenViewAsync(screenViewData);
        }
        #endregion

        #region Event
        /// <summary>
        /// Send 'event' hit.
        /// </summary>
        /// <param name="eventData">The event payload data to be sent.</param>
        public void Event(EventData eventData)
        {
            SendHit(HitType.Event, eventData);
        }

        /// <summary>
        /// Send 'event' hit.
        /// </summary>
        public void Event(string eventCategory, string eventAction)
        {
            var eventData = new EventData
            {
                EventCategory = eventCategory,
                EventAction = eventAction
            };

            Event(eventData);
        }

        /// <summary>
        /// Send 'event' hit to app property.
        /// </summary>
        public void Event(string eventCategory, string eventAction, string applicationName, string screenName, string applicationVersion)
        {
            var eventData = new AppEventData
            {
                EventCategory = eventCategory,
                EventAction = eventAction,
                ApplicationName = applicationName,
                ScreenName = screenName,
                ApplicationVersion = applicationVersion
            };

            Event(eventData);
        }
        #endregion

        #region EventAsync
        /// <summary>
        /// Send 'event' hit asynchronously.
        /// </summary>
        /// <param name="eventData">The event payload data to be sent.</param>
        public void EventAsync(EventData eventData)
        {
            SendHitAsync(HitType.Event, eventData);
        }

        /// <summary>
        /// Send 'event' hit asynchronously.
        /// </summary>
        public void EventAsync(string eventCategory, string eventAction)
        {
            var eventData = new EventData
            {
                EventCategory = eventCategory,
                EventAction = eventAction
            };

            EventAsync(eventData);
        }

        /// <summary>
        /// Send 'event' hit to app property asynchronously.
        /// </summary>
        public void EventAsync(string eventCategory, string eventAction, string applicationName, string screenName, string applicationVersion)
        {
            var eventData = new AppEventData
            {
                EventCategory = eventCategory,
                EventAction = eventAction,
                ApplicationName = applicationName,
                ScreenName = screenName,
                ApplicationVersion = applicationVersion
            };

            EventAsync(eventData);
        }
        #endregion

        #region Exception
        /// <summary>
        /// Send 'exception' hit.
        /// </summary>
        public void Exception(string exceptionDescription, string applicationName, string applicationVersion = null, bool? isFatal = null)
        {
            var exceptionData = new ExceptionData
            {
                ExceptionDescription = exceptionDescription,
                IsFatal = isFatal,
                ApplicationName = applicationName,
                ApplicationVersion = applicationVersion
            };

            Exception(exceptionData);
        }

        /// <summary>
        /// Send 'exception' hit.
        /// </summary>
        /// <param name="exceptionData">The exception payload data to be sent.</param>
        public void Exception(ExceptionData exceptionData)
        {
            SendHit(HitType.Exception, exceptionData);
        }
        #endregion

        #region ExceptionAsync
        /// <summary>
        /// Send 'exception' hit asynchronously.
        /// </summary>
        public void ExceptionAsync(string exceptionDescription, string applicationName, string applicationVersion = null, bool? isFatal = null)
        {
            var exceptionData = new ExceptionData
            {
                ExceptionDescription = exceptionDescription,
                IsFatal = isFatal,
                ApplicationName = applicationName,
                ApplicationVersion = applicationVersion
            };

            ExceptionAsync(exceptionData);
        }

        /// <summary>
        /// Send 'exception' hit asynchronously.
        /// </summary>
        /// <param name="exceptionData">The exception payload data to be sent.</param>
        public void ExceptionAsync(ExceptionData exceptionData)
        {
            SendHitAsync(HitType.Exception, exceptionData);
        }
        #endregion

        #region Social
        /// <summary>
        /// Send 'social' hit.
        /// </summary>
        public void Social(string socialNetwork, string socialAction, string socialActionTarget)
        {
            var socialData = new SocialData
            {
                SocialNetwork = socialNetwork,
                SocialAction = socialAction,
                SocialActionTarget = socialActionTarget
            };

            Social(socialData);
        }

        /// <summary>
        /// Send 'social' hit.
        /// </summary>
        /// <param name="socialData">The social payload data to be sent.</param>
        public void Social(SocialData socialData)
        {
            SendHit(HitType.Social, socialData);
        }
        #endregion

        #region SocialAsync
        /// <summary>
        /// Send 'social' hit asynchronously.
        /// </summary>
        public void SocialAsync(string socialNetwork, string socialAction, string socialActionTarget)
        {
            var socialData = new SocialData
            {
                SocialNetwork = socialNetwork,
                SocialAction = socialAction,
                SocialActionTarget = socialActionTarget
            };

            SocialAsync(socialData);
        }

        /// <summary>
        /// Send 'social' hit asynchronously.
        /// </summary>
        /// <param name="socialData">The social payload data to be sent.</param>
        public void SocialAsync(SocialData socialData)
        {
            SendHitAsync(HitType.Social, socialData);
        }
        #endregion
        #endregion

        #region GetPayloadData
        /// <summary>
        /// Get payload data portion related to a client
        /// </summary>
        private string GetPayloadData()
        {
            var stringBuilder = new StringBuilder(512);

            // v=
            stringBuilder.Append(VersionParameter);

            // &tid=
            stringBuilder.Append(TrackingIdParameter);
            stringBuilder.Append(TrackingId);

            // &cid=
            stringBuilder.Append(ClientIdParameter);
            stringBuilder.Append(ClientId ?? Guid.NewGuid());

            if (_userAgentEncoded.NotNullOrEmpty())
            {
                // &ua=
                stringBuilder.Append(UserAgentParameter);
                stringBuilder.Append(_userAgentEncoded);
            }

            if (_userLanguage.NotNullOrEmpty())
            {
                // &ul=
                stringBuilder.Append(UserLanguageParameter);
                stringBuilder.Append(_userLanguage);
            }

            return stringBuilder.ToString();
        }
        #endregion

        #region SendHit
        /// <summary>
        /// Send hit.
        /// </summary>
        public void SendHit(HitType hitType, PayloadData payload)
        {
            var payloadData = GetPayloadData() + payload.GetPayloadData();
            SendHit(payloadData);
        }

        /// <summary>
        /// Send hit with pre-formatted payload data.
        /// </summary>
        /// <param name="payloadData">The complete payload data to send. It should be already encoded and formatted according to the documentation.</param>
        public void SendHit(string payloadData)
        {
            var apiEndpointUrl = UseSsl ? ApiEndpointUrlSsl : ApiEndpointUrl;

            if (Method == "POST")
            {
                if (PayloadData.ThrowExceptions && payloadData.Length > MaxLengthBytesPostBody)
                {
                    throw new ArgumentOutOfRangeException(string.Format("The body of the post request exceeds {0} bytes", MaxLengthBytesPostBody), null as Exception);
                }

                HttpHelper.SendPostRequest(apiEndpointUrl, payloadData, PayloadData.Encoding);
            }
            else
            {
                var endpointUrl = apiEndpointUrl + "?" + payloadData;

                if (PayloadData.ThrowExceptions && endpointUrl.Length > MaxLengthBytesUrl)
                {
                    throw new ArgumentOutOfRangeException(string.Format("The length of the entire encoded URL must be no longer than {0} Bytes", MaxLengthBytesUrl), null as Exception);
                }

                HttpHelper.SendGetRequest(endpointUrl);
            }
        }
        #endregion

        #region SendHitAsync
        /// <summary>
        /// Send hit asynchronously with pre-formatted payload data.
        /// </summary>
        public void SendHitAsync(HitType hitType, PayloadData payload)
        {
            var payloadData = GetPayloadData() + payload.GetPayloadData();
            SendHitAsync(payloadData);
        }

        /// <summary>
        /// Send hit asynchronously with pre-formatted payload data.
        /// </summary>
        /// <param name="payloadData">The complete payload data to send. It should be already encoded and formatted according to the documentation.</param>
        public void SendHitAsync(string payloadData)
        {
            var apiEndpointUrl = UseSsl ? ApiEndpointUrlSsl : ApiEndpointUrl;

            if (Method == "POST")
            {
                if (PayloadData.ThrowExceptions && payloadData.Length > MaxLengthBytesPostBody)
                {
                    throw new ArgumentOutOfRangeException(string.Format("The body of the post request exceeds {0} bytes", MaxLengthBytesPostBody), null as Exception);
                }

                HttpHelper.SendPostRequestAsync(apiEndpointUrl, payloadData, PayloadData.Encoding);
            }
            else
            {
                var endpointUrl = apiEndpointUrl + "?" + payloadData;

                if (PayloadData.ThrowExceptions && endpointUrl.Length > MaxLengthBytesUrl)
                {
                    throw new ArgumentOutOfRangeException(string.Format("The length of the entire encoded URL must be no longer than {0} Bytes", MaxLengthBytesUrl), null as Exception);
                }

                HttpHelper.SendGetRequestAsync(endpointUrl);
            }
        }
        #endregion
        #endregion
    }
}
