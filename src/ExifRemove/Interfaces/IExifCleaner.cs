// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IExifCleaner.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exif cleaner interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExifRemove.Interfaces;

/// <summary>
/// The exif cleaner interface.
/// </summary>
public interface IExifCleaner
{
    /// <summary>
    /// The exif clean completed event handler.
    /// </summary>
    event EventHandler<ExifCleanCompletedEventArgs> ExifCleanCompleted;

    /// <summary>
    /// The exif clean progress event handler.
    /// </summary>
    event EventHandler<ExifCleanProgressEventArgs> ExifCleanProgress;

    /// <summary>
    /// The exception thrown event handler.
    /// </summary>
    event EventHandler<ExceptionThrownEventArgs> ExceptionThrown;

    /// <summary>
    /// The start method.
    /// </summary>
    void Start();
}
