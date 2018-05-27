using System;
using ImageService.Logging.Modal;

namespace ImageService.Logging
{
    public class LogMessage
    {
        #region Members
        private MessageTypeEnum _type;
        #endregion

        #region Properties

        public string Message { get; set; }

        public string Type
        {
            get { return Enum.GetName(typeof(MessageTypeEnum), _type); }
            set { _type = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), value); }
        }
        #endregion
    }
}