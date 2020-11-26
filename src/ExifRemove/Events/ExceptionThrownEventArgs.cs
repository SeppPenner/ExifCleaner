// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionThrownEventArgs.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   The exception thrown event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExifRemove.Events
{
    using System;

    /// <inheritdoc cref="EventArgs"/>
    /// <summary>
    /// The exception thrown event args.
    /// </summary>
    /// <seealso cref="EventArgs"/>
    public class ExceptionThrownEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionThrownEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ExceptionThrownEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Message { get; set; }
    }
}