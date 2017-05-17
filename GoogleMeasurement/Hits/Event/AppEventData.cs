using System;
using System.Text;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Represents 'event' payload data being sent to Google Analytics 'app' property.
    /// </summary>
    public sealed class AppEventData : EventData
    {
        #region Fields
        private readonly AppProperty _appProperty = new AppProperty();
        #endregion

        #region Properties
        #region ScreenName
        /// <summary>
        /// This parameter is optional on web properties, and required on mobile properties
        /// for screenview hits, where it is used for the 'Screen Name' of the screenview hit.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string ScreenName
        {
            get { return _appProperty.ScreenName; }
            set { _appProperty.ScreenName = value; }
        }
        #endregion

        #region ApplicationName
        /// <summary>
        /// Specifies the application name. 
        /// This field is required for all hit types sent to app properties.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string ApplicationName
        {
            get { return _appProperty.ApplicationName; }
            set { _appProperty.ApplicationName = value; }
        }
        #endregion

        #region ApplicationVersion
        /// <summary>
        /// Specifies the application version.
        /// <para>This parameter is optional.</para>
        /// </summary>
        public string ApplicationVersion
        {
            get { return _appProperty.ApplicationVersion; }
            set { _appProperty.ApplicationVersion = value; }
        }
        #endregion
        #endregion

        /// <summary>
        /// Gets payload data related to <c>event</c> hit.
        /// </summary>
        /// <returns>String represented the portion of payload data needs to be send to GA.</returns>
        internal override string GetPayloadData()
        {
            var eventPayloadData = base.GetPayloadData();
            var stringBuilder = new StringBuilder(2048);

            if (_appProperty.ApplicationNameEncoded.NotNullOrEmpty())
            {
                // &an=
                stringBuilder.Append(AppProperty.ApplicationNameParameter);
                stringBuilder.Append(_appProperty.ApplicationNameEncoded);
            }
            else if (ThrowExceptions)
            {
                throw new InvalidOperationException("\"ApplicationName\" property is required for all hit types sent to app properties");
            }

            if (_appProperty.ScreenNameEncoded.NotNullOrEmpty())
            {
                // &cd=
                stringBuilder.Append(AppProperty.ScreenNameParameter);
                stringBuilder.Append(_appProperty.ScreenNameEncoded);
            }

            if (_appProperty.ApplicationVersion.NotNullOrEmpty())
            {
                // &av=
                stringBuilder.Append(AppProperty.ApplicationVersionParameter);
                stringBuilder.Append(_appProperty.ApplicationVersion);
            }

            stringBuilder.Append(eventPayloadData);

            return stringBuilder.ToString();
        }
    }
}
