using System;

namespace ExifRemove.Events
{
    public class ExifCleanCompletedEventArgs : EventArgs
    {
        public ExifCleanCompletedEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}