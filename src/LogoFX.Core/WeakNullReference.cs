namespace LogoFX.Core
{
    /// Provides a weak reference to a null target object, which, unlike
    /// other weak references, is always considered to be alive. This
    /// facilitates handling null dictionary values, which are perfectly
    /// legal.
    public class WeakNullReference<T> : WeakReference<T> where T : class
    {
        /// <summary>
        /// The instance of <see cref="WeakNullReference{T}"/>
        /// </summary>
        public static readonly WeakNullReference<T> Singleton = new WeakNullReference<T>();

        private WeakNullReference() : base(null) { }

        /// <summary>
        /// Gets an indication whether the object referenced by the current <see cref="WeakReference{T}"/> object has been garbage collected.
        /// </summary>
        /// 
        /// <returns>
        /// true if the object referenced by the current <see cref="WeakReference{T}"/> object has not been garbage collected and is still accessible; otherwise, false.
        /// </returns>
        public override bool IsAlive => true;
    }
}