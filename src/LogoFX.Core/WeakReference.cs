using System;

namespace LogoFX.Core
{
    /// <summary>
    /// Adds strong typing to WeakReference.Target using generics. Also,
    /// the Create factory method is used in place of a constructor
    /// to handle the case where target is null, but we want the
    /// reference to still appear to be alive.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WeakReference<T> where T : class
    {
        private readonly WeakReference _inner;

        /// <summary>
        /// Creates <see cref="WeakReference{T}"/> from the provided target.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static WeakReference<T> Create(T target)
        {
            if (target == null)
                return WeakNullReference<T>.Singleton;

            return new WeakReference<T>(target);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.ArgumentNullException">target</exception>
        protected WeakReference(T target)
        {
            if (target == null) throw new ArgumentNullException("target");
            _inner = new WeakReference(target);
        }

        /// <summary>
        /// Gets the weak reference's target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public T Target => (T)_inner.Target;

        /// <summary>
        /// Gets an indication whether the object referenced by the current <see cref="WeakReference{T}"/> object has been garbage collected.
        /// </summary>
        /// 
        /// <returns>
        /// true if the object referenced by the current <see cref="WeakReference{T}"/> object has not been garbage collected and is still accessible; otherwise, false.
        /// </returns>
        public virtual bool IsAlive => _inner.IsAlive;
    }
}