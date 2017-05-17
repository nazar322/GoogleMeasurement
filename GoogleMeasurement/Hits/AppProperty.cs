using System;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Contains common parameters related to 'Mobile app' GA property.
    /// </summary>
    internal class AppProperty
    {
        #region Constants
        #region Validation
        private const int MaxLengthBytesScreenName = 2048;
        private const int MaxLengthBytesApplicationName = 100;
        private const int MaxLengthBytesApplicationVersion = 100;
        #endregion

        #region Parameters
        internal const string ScreenNameParameter = "&cd=";
        internal const string ApplicationNameParameter = "&an=";
        internal const string ApplicationVersionParameter = "&av=";
        #endregion
        #endregion

        #region Properties
        #region ScreenName
        private string _screenName;
        private string _screenNameEncoded;

        /// <summary>
        /// This parameter is optional on web properties, and required on mobile properties
        /// for screenview hits, where it is used for the 'Screen Name' of the screenview hit.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        internal string ScreenName
        {
            get { return _screenName; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _screenName = _screenNameEncoded = value;
                }
                else
                {
                    var screenNameEncoded = value.UrlEncode();
                    if (PayloadData.ThrowExceptions && screenNameEncoded.Length > MaxLengthBytesScreenName)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"ScreenName\" property should not exceed of {0} bytes", MaxLengthBytesScreenName));
                    }

                    _screenNameEncoded = screenNameEncoded;
                    _screenName = value;
                }
            }
        }

        internal string ScreenNameEncoded
        {
            get { return _screenNameEncoded; }
        }
        #endregion

        #region ApplicationName
        private string _applicationName;
        private string _applicationNameEncoded;

        /// <summary>
        /// Specifies the application name. 
        /// This field is required for all hit types sent to app properties.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        internal string ApplicationName
        {
            get { return _applicationName; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _applicationName = _applicationNameEncoded = value;
                }
                else
                {
                    var applicationNameEncoded = value.UrlEncode();
                    if (PayloadData.ThrowExceptions && applicationNameEncoded.Length > MaxLengthBytesApplicationName)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"ApplicationName\" property should not exceed of {0} bytes", MaxLengthBytesApplicationName));
                    }

                    _applicationNameEncoded = applicationNameEncoded;
                    _applicationName = value;
                }
            }
        }

        internal string ApplicationNameEncoded
        {
            get { return _applicationNameEncoded; }
        }
        #endregion

        #region ApplicationVersion
        private string _applicationVersion;

        /// <summary>
        /// Specifies the application version.
        /// <para>This parameter is optional.</para>
        /// </summary>
        public string ApplicationVersion
        {
            get { return _applicationVersion; }
            set
            {
                if (PayloadData.ThrowExceptions && value.NotNullOrEmpty() && value.Length > MaxLengthBytesApplicationVersion)
                {
                    throw new ArgumentOutOfRangeException("value", string.Format("The length of \"ApplicationVersion\" property should not exceed of {0} bytes", MaxLengthBytesApplicationVersion));
                }

                _applicationVersion = value;
            }
        }
        #endregion
        #endregion
    }
}
