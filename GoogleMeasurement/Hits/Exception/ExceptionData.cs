using System;
using System.Text;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Represents 'exception' payload data being sent to Google Analytics.
    /// </summary>
    public class ExceptionData : PayloadData
    {
        #region Constants
        #region Validation
        private const int MaxLengthBytesExceptionDescription = 150;
        private const int MaxLengthBytesApplicationVersion = 100;
        #endregion

        #region Parameters
        private const string HitTypeParameter = "&t=exception";
        private const string ExceptionDescriptionParameter = "&exd=";
        private const string IsExceptionFatalParameter = "&exf=";
        private const string ApplicationVersionParameter = "&av=";
        #endregion
        #endregion

        #region Fields
        private readonly AppProperty _appProperty = new AppProperty();
        #endregion

        #region Properties
        #region ExceptionDescription
        private string _exceptionDescription;
        private string _exceptionDescriptionEncoded;

        /// <summary>
        /// Specifies the description of an exception.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string ExceptionDescription
        {
            get { return _exceptionDescription; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _exceptionDescription = _exceptionDescriptionEncoded = value;
                }
                else
                {
                    var exceptionDescriptionEncoded = value.UrlEncode();
                    if (ThrowExceptions && exceptionDescriptionEncoded.Length > MaxLengthBytesExceptionDescription)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"ExceptionDescription\" property should not exceed of {0} bytes", MaxLengthBytesExceptionDescription));
                    }

                    _exceptionDescriptionEncoded = exceptionDescriptionEncoded;
                    _exceptionDescription = value;
                }
            }
        }
        #endregion

        #region IsFatal
        /// <summary>
        /// Specifies whether the exception was fatal.
        /// <para>This parameter is optional.</para>
        /// </summary>
        public bool? IsFatal { get; set; }
        #endregion

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
                if (ThrowExceptions && value.NotNullOrEmpty() && value.Length > MaxLengthBytesApplicationVersion)
                {
                    throw new ArgumentOutOfRangeException("value", string.Format("The length of \"ApplicationVersion\" property should not exceed of {0} bytes", MaxLengthBytesApplicationVersion));
                }

                _applicationVersion = value;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Gets payload data related to <c>exception</c> hit.
        /// </summary>
        /// <returns>String represented the portion of payload data needs to be send to GA.</returns>
        internal override string GetPayloadData()
        {
            var basePayload = base.GetPayloadData();
            var stringBuilder = new StringBuilder(2048);

            if (_exceptionDescriptionEncoded.NotNullOrEmpty())
            {
                // &exd=
                stringBuilder.Append(ExceptionDescriptionParameter);
                stringBuilder.Append(_exceptionDescriptionEncoded);
            }
            else if (ThrowExceptions)
            {
                throw new InvalidOperationException("The value of \"ExceptionDescription\" must not be empty");
            }

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

            if (IsFatal.HasValue)
            {
                // &exf=
                stringBuilder.Append(IsExceptionFatalParameter);
                stringBuilder.Append(IsFatal.Value ? 1 : 0);
            }

            if (_applicationVersion.NotNullOrEmpty())
            {
                // &av=
                stringBuilder.Append(ApplicationVersionParameter);
                stringBuilder.Append(_applicationVersion);
            }

            // &t=
            stringBuilder.Append(HitTypeParameter);

            stringBuilder.Append(basePayload);

            return stringBuilder.ToString();
        }
    }
}
