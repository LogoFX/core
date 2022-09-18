using System.Diagnostics.CodeAnalysis;

namespace LogoFX.Client.Core
{
    /// <summary>
    /// Dispatcher-related constants.
    /// </summary>
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public static class Consts
    {
        /// <summary>
        /// The dispatcher priority
        /// </summary>
#if !WINUI3
        public const System.Windows.Threading.DispatcherPriority
            DispatcherPriority = System.Windows.Threading.DispatcherPriority.DataBind;
#else
        public const Microsoft.UI.Dispatching.DispatcherQueuePriority
			DispatcherPriority = Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal;
#endif
    }
}
