//*********************************************************
// LLaser project - MessageHelper.cs
// Created at 2013-5-15
// Author: Bob Bao jar.bob@gmail.com
//
// All rights reserved. Please follow license agreement to
// rewrite and copy this code.
//
//*********************************************************

using LLaser.Common;

namespace LLaser.Component
{
    public class MessageHelper : BindableBase
    {
        private static MessageHelper _instance;

        public static MessageHelper Current
        {
            get { return _instance ?? (_instance = new MessageHelper()); }
        }

        #region general message methods

        private string _message;
        private MessageTypes _messageType;

        public string MessageIcon
        {
            get
            {
                switch (MessageType)
                {
                    case MessageTypes.Normal:
                        return @"pack://application:,,,/LLaser;component/Image/normal.png";
                    case MessageTypes.Information:
                        return @"pack://application:,,,/LLaser;component/Image/information.png";
                    case MessageTypes.Message:
                        return @"pack://application:,,,/LLaser;component/Image/message.png";
                    case MessageTypes.Help:
                        return @"pack://application:,,,/LLaser;component/Image/help.png";
                    case MessageTypes.Error:
                        return @"pack://application:,,,/LLaser;component/Image/error.png";
                    case MessageTypes.Warning:
                        return @"pack://application:,,,/LLaser;component/Image/warning.png";
                    default:
                        return @"pack://application:,,,/LLaser;component/Image/normal.png";
                }
            }
        }

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public MessageTypes MessageType
        {
            get { return _messageType; }
            set
            {
                SetProperty(ref _messageType, value);
                OnPropertyChanged("MessageIcon");
            }
        }

        /// <summary>
        /// Set message during runtime
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public void SetMessage(string message, MessageTypes type = MessageTypes.Normal)
        {
            MessageType = type;
            Message = message;
        }

        /// <summary>
        /// Rest application global message to default status
        /// </summary>
        public void ResetMessage(string message = "")
        {
            SetMessage(message);
        }

        #endregion

    }
}
