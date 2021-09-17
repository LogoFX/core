using System;

namespace LogoFX.Core
{
    /// <summary>
    /// Helper for temporary notification suppression.
    /// </summary>
    public sealed class SuppressNotifyHelper : IDisposable
    {
        private readonly INotifyManager _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuppressNotifyHelper"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public SuppressNotifyHelper(INotifyManager source)
        {
            _source = source;
            _source.Mute();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _source.Unmute();
        }
    }
}