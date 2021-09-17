namespace LogoFX.Core
{
    /// <summary>
    /// Defines an object that manages notifications muting.
    /// </summary>
    public interface INotifyManager
    {
        /// <summary>
        /// Gets the value indicating whether the notifications are muted.
        /// </summary>
        bool IsMuted { get; }

        /// <summary>
        /// Mutes the notifications.
        /// </summary>
        void Mute();

        /// <summary>
        /// Unmutes the notifications
        /// </summary>
        void Unmute();
    }

    /// <inheritdoc />
    public sealed class NotifyManager : INotifyManager
    {
        /// <inheritdoc />
        public bool IsMuted { get; private set; }

        /// <inheritdoc />
        public void Mute()
        {
            IsMuted = true;
        }

        /// <inheritdoc />
        public void Unmute()
        {
            IsMuted = false;
        }
    }
}
