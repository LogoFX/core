using System;
using System.Reflection;

namespace LogoFX.Core
{
    /// <summary>
    /// A class used as the base class to implement weak delegates.
    /// See <see cref="WeakDelegate"/>.From method implementations to see how it works.
    /// </summary>
    public class WeakDelegateBase :
        WeakReference<object>
    {
        /// <summary>
        /// Creates this weak-delegate class based as a copy of the given 
        /// delegate handler.
        /// </summary>
        /// <param name="handler">The handler to copy information from.</param>
        public WeakDelegateBase(Delegate handler) :
            base(handler.Target)
        {
            Method = handler.GetMethodInfo();
        }

        /// <summary>
        /// Gets the method used by this delegate.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// Invokes this delegate with the given parameters.
        /// </summary>
        /// <param name="parameters">The parameters to be used by the delegate.</param>
        public void Invoke(object[] parameters)
        {
            object target = Target;
            if (target != null || Method.IsStatic)
            {
                Method.Invoke(target, parameters);
            }                
        }
    }
}