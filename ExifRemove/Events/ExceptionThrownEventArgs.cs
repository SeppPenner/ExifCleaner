using System;

namespace ExifRemove.Events
{
    public class ExceptionThrownEventArgs : EventArgs
    {
        public ExceptionThrownEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}