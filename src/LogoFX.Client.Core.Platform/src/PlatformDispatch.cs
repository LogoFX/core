using System.Diagnostics.CodeAnalysis;
#if WINUI3
using Microsoft.UI.Dispatching;
#else
using System.Windows.Threading;
#endif
using LogoFX.Client.Core;

// ReSharper disable once CheckNamespace
namespace System.Threading
{    
    /// <summary>
    /// Platform-specific dispatcher
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PlatformDispatch : IDispatch
    {
#if WINUI3
		private Action<Action, bool, DispatcherQueuePriority> _dispatch;
#else
        private Action<Action, bool, DispatcherPriority> _dispatch;
#endif
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
	        var dispatcher
#if WINUI3
		        = DispatcherQueue.GetForCurrentThread();
#else
                = Dispatcher.CurrentDispatcher;
#endif
	        if (dispatcher == null)
	        {
		        throw new InvalidOperationException("Dispatch is not initialized correctly");
	        }

	        _dispatch = (action, async, priority) =>
            {
                if (!async &&
#if WINUI3
						dispatcher.HasThreadAccess
#else
						dispatcher.CheckAccess()
#endif
                    )
                {
                    action();
                }               
                else
                {
#if WINUI3
	                dispatcher.TryEnqueue(priority, () => action());
#else
                    dispatcher.BeginInvoke(action, priority);
#endif
				}
            };
        }

        /// <inheritdoc />
        public void BeginOnUiThread(Action action)
	        => BeginOnUiThread(Consts.DispatcherPriority, action);

        /// <summary>
        /// Begins the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="priority">Desired priority</param>
        /// <param name="action">Action</param>
        public void BeginOnUiThread(
#if WINUI3
            DispatcherQueuePriority priority,
#else
	        DispatcherPriority priority,
#endif 
            Action action)
        {
            EnsureDispatch();
            _dispatch(action, true, priority);
        }

        /// <inheritdoc />
        public void OnUiThread(Action action) 
	        => OnUiThread(Consts.DispatcherPriority, action);

        /// <summary>
        /// Executes the action on the UI thread according to the specified priority
        /// </summary>
        /// <param name="priority">Desired priority</param>
        /// <param name="action">Action</param>
        public void OnUiThread(
#if WINUI3
            DispatcherQueuePriority priority, 
#else
	        DispatcherPriority priority,
#endif
			Action action)
        {
            EnsureDispatch();
            _dispatch(action, false, priority);
        }
    }
}
