using System;

namespace ExifRemove.Events
{
    public class ExifCleanProgressEventArgs : EventArgs
    {
        public ExifCleanProgressEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}