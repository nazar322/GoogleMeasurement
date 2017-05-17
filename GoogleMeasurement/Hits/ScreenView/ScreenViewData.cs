using System;
using System.Text;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Represents 'screenview' payload data being sent to Google Analytics.
    /// </summary>
    /// <remarks>This class made to work only with/for mobile GA properties.</remarks>
    public sealed class ScreenViewData : PayloadData
    {
        #region Constants
        #region Validation
        private const int MaxLengthBytesApplicationId = 150;
        private const int MaxLengthBytesApplicationInstallerId = 150;
        #endregion

        #region Parameters
        private const string HitTypeParameter = "&t=screenview";
        private const string ApplicationIdParameter = "&aid=";
        private const string ApplicationInstallerIdParameter = "&aiid=";
        #endregion
        #endregion

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

        #region ApplicationId
        private string _applicationId;

        /// <summary>
        /// Application identifier.
        /// <para>This parameter is optional.</para>
        /// </summary>
        public string ApplicationId
        {
            get { return _applicationId; }
            set
            {
                if (ThrowExceptions && value.NotNullOrEmpty() && value.Length > MaxLengthBytesApplicationId)
                {
                    throw new ArgumentOutOfRangeException("value", string.Format("The length of \"ApplicationId\" property should not exceed of {0} bytes", MaxLengthBytesApplicationId));
                }

                _applicationId = value;
            }
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

        #region ApplicationInstallerId
        private string _applicationInstallerId;

        /// <summary>
        /// Application installer identifier.
        /// <para>This parameter is optional.</para>
        /// </summary>
        public string ApplicationInstallerId
        {
            get { return _applicationInstallerId; }
            set
            {
                if (ThrowExceptions && value.NotNullOrEmpty() && value.Length > MaxLengthBytesApplicationInstallerId)
                {
                    throw new ArgumentOutOfRangeException("value", string.Format("The length of \"ApplicationInstallerId\" property should not exceed of {0} bytes", MaxLengthBytesApplicationInstallerId));
                }

                _applicationInstallerId = value;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Gets payload data related to <c>screenview</c> hit.
        /// </summary>
        /// <returns>String represented the portion of payload data needs to be send to GA.</returns>
        internal override string GetPayloadData()
        {
            var basePayload = base.GetPayloadData();
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
            else if (ThrowExceptions)
            {
                throw new InvalidOperationException("\"ScreenName\" property is required on mobile properties for screenview hits");
            }

            // &t=
            stringBuilder.Append(HitTypeParameter);

            if (_appProperty.ApplicationVersion.NotNullOrEmpty())
            {
                // &av=
                stringBuilder.Append(AppProperty.ApplicationVersionParameter);
                stringBuilder.Append(_appProperty.ApplicationVersion);
            }

            if (_applicationId.NotNullOrEmpty())
            {
                // &aid=
                stringBuilder.Append(ApplicationIdParameter);
                stringBuilder.Append(_applicationId);
            }

            if (_applicationInstallerId.NotNullOrEmpty())
            {
                // &aiid=
                stringBuilder.Append(ApplicationInstallerIdParameter);
                stringBuilder.Append(_applicationInstallerId);
            }

            stringBuilder.Append(basePayload);

            return stringBuilder.ToString();
        }
    }
}
