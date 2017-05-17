using System;
using System.Text;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Represents 'social' payload data being sent to Google Analytics.
    /// </summary>
    public sealed class SocialData : PayloadData
    {
        #region Constants
        #region Validation
        private const int MaxLengthBytesSocialNetwork = 50;
        private const int MaxLengthBytesSocialAction = 50;
        private const int MaxLengthBytesSocialActionTarget = 2048;
        #endregion

        #region Parameters
        private const string HitTypeParameter = "&t=social";
        private const string SocialNetworkParameter = "&sn=";
        private const string SocialActionParameter = "&sa=";
        private const string SocialActionTargetParameter = "&st=";
        #endregion
        #endregion

        #region Properties
        #region SocialNetwork
        private string _socialNetwork;
        private string _socialNetworkEncoded;

        /// <summary>
        /// Specifies the social network, for example Facebook or Google Plus. Must not be empty.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string SocialNetwork
        {
            get { return _socialNetwork; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _socialNetwork = _socialNetworkEncoded = value;
                }
                else
                {
                    var socialNetworkEncoded = value.UrlEncode();
                    if (ThrowExceptions && socialNetworkEncoded.Length > MaxLengthBytesSocialNetwork)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"SocialNetwork\" property should not exceed of {0} bytes", MaxLengthBytesSocialNetwork));
                    }

                    _socialNetworkEncoded = socialNetworkEncoded;
                    _socialNetwork = value;
                }
            }
        }
        #endregion

        #region SocialAction
        private string _socialAction;
        private string _socialActionEncoded;

        /// <summary>
        /// Specifies the social interaction action. Must not be empty.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string SocialAction
        {
            get { return _socialAction; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _socialAction = _socialActionEncoded = value;
                }
                else
                {
                    var socialActionEncoded = value.UrlEncode();
                    if (ThrowExceptions && socialActionEncoded.Length > MaxLengthBytesSocialAction)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"SocialAction\" property should not exceed of {0} bytes", MaxLengthBytesSocialAction));
                    }

                    _socialActionEncoded = socialActionEncoded;
                    _socialAction = value;
                }
            }
        }
        #endregion

        #region SocialActionTarget
        private string _socialActionTarget;
        private string _socialActionTargetEncoded;

        /// <summary>
        /// Specifies the target of a social interaction. This value is typically a URL but can be any text.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string SocialActionTarget
        {
            get { return _socialActionTarget; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _socialActionTarget = _socialActionTargetEncoded = value;
                }
                else
                {
                    var socialActionTargetEncoded = value.UrlEncode();
                    if (ThrowExceptions && socialActionTargetEncoded.Length > MaxLengthBytesSocialActionTarget)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"SocialActionTarget\" property should not exceed of {0} bytes", MaxLengthBytesSocialActionTarget));
                    }

                    _socialActionTargetEncoded = socialActionTargetEncoded;
                    _socialActionTarget = value;
                }
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Gets payload data related to <c>social</c> hit.
        /// </summary>
        /// <returns>String represented the portion of payload data needs to be send to GA.</returns>
        internal override string GetPayloadData()
        {
            var basePayload = base.GetPayloadData();
            var stringBuilder = new StringBuilder(2048);

            if (_socialNetworkEncoded.NotNullOrEmpty())
            {
                // &sn=
                stringBuilder.Append(SocialNetworkParameter);
                stringBuilder.Append(_socialNetworkEncoded);
            }
            else if (ThrowExceptions)
            {
                throw new InvalidOperationException("The value of \"SocialNetwork\" must not be empty");
            }

            if (_socialActionEncoded.NotNullOrEmpty())
            {
                // &sa=
                stringBuilder.Append(SocialActionParameter);
                stringBuilder.Append(_socialActionEncoded);
            }
            else if (ThrowExceptions)
            {
                throw new InvalidOperationException("The value of \"SocialAction\" must not be empty");
            }

            if (_socialActionTargetEncoded.NotNullOrEmpty())
            {
                // &st=
                stringBuilder.Append(SocialActionTargetParameter);
                stringBuilder.Append(_socialActionTargetEncoded);
            }
            else if (ThrowExceptions)
            {
                throw new InvalidOperationException("The value of \"SocialActionTarget\" must not be empty");
            }

            // &t=
            stringBuilder.Append(HitTypeParameter);

            stringBuilder.Append(basePayload);

            return stringBuilder.ToString();
        }
    }
}
