using System;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Contains common parameters related to 'Web' GA property.
    /// </summary>
    internal class WebProperty
    {
        #region Constants
        #region Validation
        private const int MaxLengthBytesDocumentLocation = 2048;
        private const int MaxLengthBytesDocumentHostName = 100;
        private const int MaxLengthBytesDocumentPath = 2048;
        private const int MaxLengthBytesDocumentTitle = 1500;
        #endregion

        #region Parameters
        internal const string DocumentLocationParameter = "&dl=";
        internal const string DocumentHostNameParameter = "&dh=";
        internal const string DocumentPathParameter = "&dp=";
        internal const string DocumentTitleParameter = "&dt=";
        #endregion
        #endregion

        #region Properties
        #region DocumentLocation
        private string _documentLocation;
        private string _documentLocationEncoded;

        /// <summary>
        /// Use this parameter to send the full URL (document location) of the page on which content resides.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// <para>This parameter is optional.</para>
        /// </summary>
        internal string DocumentLocation
        {
            get { return _documentLocation; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _documentLocation = _documentLocationEncoded = value;
                }
                else
                {
                    var documentLocationEncoded = value.UrlEncode();
                    if (PayloadData.ThrowExceptions && documentLocationEncoded.Length > MaxLengthBytesDocumentLocation)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"DocumentLocation\" property should not exceed of {0} bytes", MaxLengthBytesDocumentLocation));
                    }

                    _documentLocationEncoded = documentLocationEncoded;
                    _documentLocation = value;
                }
            }
        }

        internal string DocumentLocationEncoded
        {
            get { return _documentLocationEncoded; }
        }
        #endregion

        #region DocumentHostName
        private string _documentHostName;
        private string _documentHostNameEncoded;

        /// <summary>
        /// Specifies the hostname from which content was hosted.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// <para>This parameter is optional.</para>
        /// </summary>
        internal string DocumentHostName
        {
            get { return _documentHostName; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _documentHostName = _documentHostNameEncoded = value;
                }
                else
                {
                    var documentHostNameEncoded = value.UrlEncode();
                    if (PayloadData.ThrowExceptions && documentHostNameEncoded.Length > MaxLengthBytesDocumentHostName)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"DocumentHostName\" property should not exceed of {0} bytes", MaxLengthBytesDocumentHostName));
                    }

                    _documentHostNameEncoded = documentHostNameEncoded;
                    _documentHostName = value;
                }
            }
        }

        internal string DocumentHostNameEncoded
        {
            get { return _documentHostNameEncoded; }
        }
        #endregion

        #region DocumentPath
        private string _documentPath;
        private string _documentPathEncoded;

        /// <summary>
        /// The path portion of the page URL.
        /// <para>Should begin with '/'.</para>
        /// <para>Either <c>DocumentLocation</c> or both <c>DocumentHostName</c> and <c>DocumentPath</c> have to be specified for the hit to be valid.</para>
        /// <para>This parameter is optional.</para>
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        internal string DocumentPath
        {
            get { return _documentPath; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _documentPath = _documentPathEncoded = value;
                }
                else
                {
                    if (PayloadData.ThrowExceptions && value[0] != '/')
                        throw new ArgumentException("\"DocumentPath\" should begin with '/'", "value");

                    var documentPathEncoded = value.UrlEncode();
                    if (PayloadData.ThrowExceptions && documentPathEncoded.Length > MaxLengthBytesDocumentPath)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"DocumentPath\" property should not exceed of {0} bytes", MaxLengthBytesDocumentPath));
                    }

                    _documentPathEncoded = documentPathEncoded;
                    _documentPath = value;
                }
            }
        }

        internal string DocumentPathEncoded
        {
            get { return _documentPathEncoded; }
        }
        #endregion

        #region DocumentTitle
        private string _documentTitle;
        private string _documentTitleEncoded;

        /// <summary>
        /// The title of the page or document.
        /// <para>This parameter is optional.</para>
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        internal string DocumentTitle
        {
            get { return _documentTitle; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _documentTitle = _documentTitleEncoded = value;
                }
                else
                {
                    var documentTitleEncoded = value.UrlEncode();
                    if (PayloadData.ThrowExceptions && documentTitleEncoded.Length > MaxLengthBytesDocumentTitle)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"DocumentTitle\" property should not exceed of {0} bytes", MaxLengthBytesDocumentTitle));
                    }

                    _documentTitleEncoded = documentTitleEncoded;
                    _documentTitle = value;
                }
            }
        }

        internal string DocumentTitleEncoded
        {
            get { return _documentTitleEncoded; }
        }
        #endregion
        #endregion
    }
}
