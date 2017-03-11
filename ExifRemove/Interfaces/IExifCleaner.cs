using System;
using ExifRemove.Events;

namespace ExifRemove.Interfaces
{
    public interface IExifCleaner
    {
        event EventHandler<ExifCleanCompletedEventArgs> ExifCleanCompleted;

        event EventHandler<ExifCleanProgressEventArgs> ExifCleanProgress;

        event EventHandler<ExceptionThrownEventArgs> ExceptionThrown;

        void Start();
    }
}