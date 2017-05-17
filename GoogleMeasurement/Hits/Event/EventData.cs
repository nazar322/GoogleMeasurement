using System;
using System.Text;

namespace Google.Data.Measurement
{
    /// <summary>
    /// Represents 'event' payload data being sent to Google Analytics.
    /// </summary>
    public class EventData : PayloadData
    {
        #region Constants
        #region Validation
        private const int MaxLengthBytesEventCategory = 150;
        private const int MaxLengthBytesEventAction = 500;
        private const int MaxLengthBytesEventLabel = 500;
        #endregion

        #region Parameters
        private const string HitTypeParameter = "&t=event";
        private const string EventCategoryParameter = "&ec=";
        private const string EventActionParameter = "&ea=";
        private const string EventLabelParameter = "&el=";
        private const string EventValueParameter = "&ev=";
        #endregion
        #endregion

        #region Properties
        #region EventCategory
        private string _eventCategory;
        private string _eventCategoryEncoded;

        /// <summary>
        /// Specifies the event category. Must not be empty.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string EventCategory
        {
            get { return _eventCategory; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _eventCategory = _eventCategoryEncoded = value;
                }
                else
                {
                    var eventCategoryEncoded = value.UrlEncode();
                    if (ThrowExceptions && eventCategoryEncoded.Length > MaxLengthBytesEventCategory)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"EventCategory\" property should not exceed of {0} bytes", MaxLengthBytesEventCategory));
                    }

                    _eventCategoryEncoded = eventCategoryEncoded;
                    _eventCategory = value;
                }
            }
        }
        #endregion

        #region EventAction
        private string _eventAction;
        private string _eventActionEncoded;

        /// <summary>
        /// Specifies the event action. Must not be empty.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// </summary>
        public string EventAction
        {
            get { return _eventAction; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _eventAction = _eventActionEncoded = value;
                }
                else
                {
                    var eventActionEncoded = value.UrlEncode();
                    if (ThrowExceptions && eventActionEncoded.Length > MaxLengthBytesEventAction)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"EventAction\" property should not exceed of {0} bytes", MaxLengthBytesEventAction));
                    }

                    _eventActionEncoded = eventActionEncoded;
                    _eventAction = value;
                }
            }
        }
        #endregion

        #region EventLabel
        private string _eventLabel;
        private string _eventLabelEncoded;

        /// <summary>
        /// Specifies the event label.
        /// <para>The value of this parameter is auto URL encoded.</para>
        /// <para>This parameter is optional.</para>
        /// </summary>
        public string EventLabel
        {
            get { return _eventLabel; }
            set
            {
                if (value.IsNullOrEmpty())
                {
                    _eventLabel = _eventLabelEncoded = value;
                }
                else
                {
                    var eventLabelEncoded = value.UrlEncode();
                    if (ThrowExceptions && eventLabelEncoded.Length > MaxLengthBytesEventLabel)
                    {
                        throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"EventLabel\" property should not exceed of {0} bytes", MaxLengthBytesEventLabel));
                    }

                    _eventLabelEncoded = eventLabelEncoded;
                    _eventLabel = value;
                }
            }
        }
        #endregion

        #region EventValue
        /// <summary>
        /// Specifies the event value.
        /// <para>This parameter is optional.</para>
        /// </summary>
        public uint? EventValue { get; set; }
        #endregion
        #endregion

        /// <summary>
        /// Gets payload data related to <c>event</c> hit.
        /// </summary>
        /// <returns>String represented the portion of payload data needs to be send to GA.</returns>
        internal override string GetPayloadData()
        {
            var basePayload = base.GetPayloadData();
            var stringBuilder = new StringBuilder(2048);

            if (_eventCategoryEncoded.NotNullOrEmpty())
            {
                // &ec=
                stringBuilder.Append(EventCategoryParameter);
                stringBuilder.Append(_eventCategoryEncoded);
            }
            else if (ThrowExceptions)
            {
                throw new InvalidOperationException("The value of \"EventCategory\" must not be empty");
            }

            if (_eventActionEncoded.NotNullOrEmpty())
            {
                // &ea=
                stringBuilder.Append(EventActionParameter);
                stringBuilder.Append(_eventActionEncoded);
            }
            else if (ThrowExceptions)
            {
                throw new InvalidOperationException("The value of \"EventAction\" must not be empty");
            }

            // &t=
            stringBuilder.Append(HitTypeParameter);

            if (_eventLabelEncoded.NotNullOrEmpty())
            {
                // &el=
                stringBuilder.Append(EventLabelParameter);
                stringBuilder.Append(_eventLabelEncoded);
            }

            if (EventValue.HasValue)
            {
                // &ev=
                stringBuilder.Append(EventValueParameter);
                stringBuilder.Append(EventValue);
            }

            stringBuilder.Append(basePayload);

            return stringBuilder.ToString();
        }
    }
}
