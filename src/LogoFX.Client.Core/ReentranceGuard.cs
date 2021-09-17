using System;

namespace LogoFX.Client.Core
{
    /// <summary>
    /// Represents a semaphore for locking user interface updating.
    /// </summary>
    public class ReentranceGuard
    {
        /// <summary>
        /// Represents an automatic reference counter for <see cref="ReentranceGuard"/> class.
        /// </summary>
        class Raiser : IDisposable
        {            
            private readonly ReentranceGuard _reentranceGuard;
            /// <summary>
            /// Initializes a new instance of the <see cref="Raiser"/> class.
            /// </summary>
            /// <param name="owner">The owner of the instance.</param>
            public Raiser(ReentranceGuard owner)
            {
                _reentranceGuard = owner;
                _reentranceGuard.Counter++;
            }
            /// <inheritdoc />            
            public void Dispose()
            {
                _reentranceGuard.Counter--;
            }
        }

        /// <summary>
        /// Gets automatic reference counter.
        /// </summary>
        public int Counter { get; private set; } = -1;

        /// <summary>
        /// Gets user interface lock flag based on the <see cref="Counter"/> value.
        /// </summary>
        public bool IsLocked => Counter > 0;

        /// <summary>
        /// Increments the counter of the references.
        /// </summary>
        /// <returns>The object, which decrements the reference on its disposal.</returns>
        public IDisposable Raise()
        {
            return new Raiser(this);
        }
    }
}
