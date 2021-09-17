#if NETFX_CORE || WINDOWS_UWP
using Windows.UI.Core;
#endif

namespace LogoFX.Client.Core
{
    /// <summary>
    /// Dispatcher-related constants.
    /// </summary>
    public static class Consts
    {
        /// <summary>
        /// The dispatcher priority
        /// </summary>
        public const
#if NET || NETCORE
            System.Windows.Threading.DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority
#endif
            DispatcherPriority =
#if NET || NETCORE
            System.Windows.Threading.DispatcherPriority.DataBind
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority.Normal
#endif
        ;
    }
}
