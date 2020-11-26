// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExifCleanProgressEventArgs.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exif clean progress event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExifRemove.Events
{
    using System;

    /// <inheritdoc cref="EventArgs"/>
    /// <summary>
    /// The exif clean progress event args.
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class ExifCleanProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExifCleanProgressEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ExifCleanProgressEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}