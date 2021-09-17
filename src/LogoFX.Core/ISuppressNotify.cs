using System;

namespace LogoFX.Core
{
    /// <summary>
    /// Designates object than can suppress some notification.
    /// </summary>
    public interface ISuppressNotify
    {
        /// <summary>
        /// Gets the suppress notify.
        /// To be used in <c>using</c> statement.
        /// </summary>
        IDisposable SuppressNotify { get; }
    }
}