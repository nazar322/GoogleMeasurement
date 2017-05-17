using System.Text;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Represents 'event' payload data being sent to Google Analytics 'web' property.
    /// </summary>
    public sealed class WebEventData : EventData
    {
        #region Fields
        private readonly WebProperty _webProperty = new WebProperty();
        #endregion

        #region Properties
        #region DocumentLocation
        /// <summary>
        /// Use this parameter to send the full URL (document location) of the page on which content resides.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// <para>This parameter is optional.</para>
        /// </summary>
        public string DocumentLocation
        {
            get { return _webProperty.DocumentLocation; }
            set { _webProperty.DocumentLocation = value; }
        }
        #endregion

        #region DocumentHostName
        /// <summary>
        /// Specifies the hostname from which content was hosted.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// <para>This parameter is optional.</para>
        /// </summary>
        public string DocumentHostName
        {
            get { return _webProperty.DocumentHostName; }
            set { _webProperty.DocumentHostName = value; }
        }
        #endregion

        #region DocumentPath
        /// <summary>
        /// The path portion of the page URL.
        /// <para>Should begin with '/'.</para>
        /// <para>Either <c>DocumentLocation</c> or both <c>DocumentHostName</c> and <c>DocumentPath</c> have to be specified for the hit to be valid.</para>
        /// <para>This parameter is optional.</para>
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string DocumentPath
        {
            get { return _webProperty.DocumentPath; }
            set { _webProperty.DocumentPath = value; }
        }
        #endregion

        #region DocumentTitle
        /// <summary>
        /// The title of the page or document.
        /// <para>This parameter is optional.</para>
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string DocumentTitle
        {
            get { return _webProperty.DocumentTitle; }
            set { _webProperty.DocumentTitle = value; }
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

            if (_webProperty.DocumentLocationEncoded.NotNullOrEmpty())
            {
                // &dl=
                stringBuilder.Append(WebProperty.DocumentLocationParameter);
                stringBuilder.Append(_webProperty.DocumentLocationEncoded);
            }

            if (_webProperty.DocumentHostNameEncoded.NotNullOrEmpty())
            {
                // &dh=
                stringBuilder.Append(WebProperty.DocumentHostNameParameter);
                stringBuilder.Append(_webProperty.DocumentHostNameEncoded);
            }

            if (_webProperty.DocumentPathEncoded.NotNullOrEmpty())
            {
                // &dp=
                stringBuilder.Append(WebProperty.DocumentPathParameter);
                stringBuilder.Append(_webProperty.DocumentPathEncoded);
            }

            if (_webProperty.DocumentTitleEncoded.NotNullOrEmpty())
            {
                // &dt=
                stringBuilder.Append(WebProperty.DocumentTitleParameter);
                stringBuilder.Append(_webProperty.DocumentTitleEncoded);
            }

            stringBuilder.Append(eventPayloadData);

            return stringBuilder.ToString();
        }
    }
}
