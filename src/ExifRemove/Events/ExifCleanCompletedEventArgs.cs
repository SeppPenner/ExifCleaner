// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExifCleanCompletedEventArgs.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exif clean completed event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExifRemove.Events;

/// <inheritdoc cref="EventArgs"/>
/// <summary>
/// The exif clean completed event args.
/// </summary>
/// <seealso cref="EventArgs"/>
public class ExifCleanCompletedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExifCleanCompletedEventArgs"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public ExifCleanCompletedEventArgs(string message)
    {
        this.Message = message;
    }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    public string Message { get; set; }
}
