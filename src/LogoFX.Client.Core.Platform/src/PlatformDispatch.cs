#if WINDOWS_UWP || NETFX_CORE
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
#endif
#if NET || NETCORE
using System.Windows.Threading;
#endif

using LogoFX.Client.Core;

// ReSharper disable once CheckNamespace
namespace System.Threading
{    
    /// <summary>
    /// Platform-specific dispatcher
    /// </summary>
    public class PlatformDispatch : IDispatch
    {
        private Action<Action, bool,
#if NET || NETCORE
            DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority
#endif
            > _dispatch;        

        private void EnsureDispatch()
        {
            if (_dispatch == null)
            {
                throw new InvalidOperationException("Dispatch is not initialized correctly");
            }
        }

        /// <summary>
        /// Initializes the framework using the current dispatcher.
        /// </summary>
        public void InitializeDispatch()
        {
#if NET || NETCORE
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (dispatcher == null)
                throw new InvalidOperationException("Dispatch is not initialized correctly");
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcher dispatcher = new UserControl().Dispatcher;            
#endif                    
            _dispatch = (action, @async, priority) =>
            {
#if NET || NETCORE
                if (!@async && dispatcher.CheckAccess())
#else
                if (!@async)
#endif
                {
                    action();
                }               
                else
                {
#if NET || NETCORE
                    dispatcher.BeginInvoke(action, priority);
#endif
#if NETFX_CORE || WINDOWS_UWP
                    dispatcher.RunAsync(priority, () => action());
#endif                                       
                }
            };
        }

        /// <inheritdoc />
        public void BeginOnUiThread(Action action)
        {
            BeginOnUiThread(Consts.DispatcherPriority, action);            
        }

        /// <summary>
        /// Begins the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="priority">Desired priority</param>
        /// <param name="action">Action</param>
        public void BeginOnUiThread(
#if NET || NETCORE
            DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority
#endif
            priority, Action action)
        {
            EnsureDispatch();
            _dispatch(action, true, priority);
        }

        /// <inheritdoc />
        public void OnUiThread(Action action)
        {
            OnUiThread(Consts.DispatcherPriority, action);
        }

        /// <summary>
        /// Executes the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="priority">Desired priority</param>
        /// <param name="action">Action</param>
        public void OnUiThread(
#if NET || NETCORE
            DispatcherPriority
#endif
#if NETFX_CORE || WINDOWS_UWP
            CoreDispatcherPriority
#endif 
            priority, Action action)
        {
            EnsureDispatch();
            _dispatch(action, false, priority);
        }
    }
}
