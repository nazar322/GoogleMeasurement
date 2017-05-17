using System.Text;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Represents payload data being sent to Google Analytics.
    /// </summary>
    /// <remarks>This is base class for payload data for all hit types.</remarks>
    public abstract class PayloadData
    {
        #region Constants
        #region Parameters
        private const string DataSourceParameter = "&ds=";
        #endregion
        #endregion

        #region Properties
        #region Encoding
        /// <summary>
        /// Default encoding.
        /// <para>Google Measurement Protocol only accepts strings encoded in UTF-8.</para>
        /// </summary>
        internal static Encoding Encoding { get; private set; }
        #endregion

        #region ThrowExceptions
        /// <summary>
        /// Throw exceptions on payload validation errors.
        /// </summary>
        internal static bool ThrowExceptions { get; set; }
        #endregion

        #region DataSource
        private string _dataSource;
        private string _dataSourceEncoded;

        /// <summary>
        /// Indicates the data source of the hit.
        /// <para>This parameter is optional.</para>
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string DataSource
        {
            get { return _dataSource; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _dataSource = _dataSourceEncoded = value;
                }
                else
                {
                    _dataSourceEncoded = value.UrlEncode();
                    _dataSource = value;
                }
            }
        }
        #endregion
        #endregion

        #region Constructors
        static PayloadData()
        {
            Encoding = Encoding.UTF8;
            ThrowExceptions = true;
        }
        #endregion

        /// <summary>
        /// Gets payload data related to hit type.
        /// </summary>
        /// <returns>String represented the portion of payload data needs to be send to GA.</returns>
        internal virtual string GetPayloadData()
        {
            var stringBuilder = new StringBuilder(_dataSourceEncoded.GetSafeLength());

            if (_dataSourceEncoded.NotNullOrEmpty())
            {
                // &ds=
                stringBuilder.Append(DataSourceParameter);
            }

            return stringBuilder.ToString();
        }
    }
}
