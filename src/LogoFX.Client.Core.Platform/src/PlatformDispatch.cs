using System.Diagnostics.CodeAnalysis;
using LogoFX.Client.Core;

#if !WINUI3
using System.Windows.Threading;
#endif

#if WINUI3
using Microsoft.UI.Dispatching;
#endif

// ReSharper disable once CheckNamespace
namespace System.Threading
{    
    /// <summary>
    /// Platform-specific dispatcher
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PlatformDispatch : IDispatch
    {
#if !WINUI3
        private Action<Action, bool, DispatcherPriority> _dispatch;
#else
		private Action<Action, bool, DispatcherQueuePriority> _dispatch;
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
#if !WINUI3
                = Dispatcher.CurrentDispatcher;
#else
		        = DispatcherQueue.GetForCurrentThread();
#endif
            if (dispatcher == null)
                throw new InvalidOperationException("Dispatch is not initialized correctly");
            _dispatch = (action, async, priority) =>
            {
                if (!async &&
#if !WINUI3
						dispatcher.CheckAccess();
#else
						dispatcher.HasThreadAccess
#endif
                    )
                {
                    action();
                }               
                else
                {
#if !WINUI3
                    dispatcher.TryEnqueue(action, priority);
#else
	                dispatcher.TryEnqueue(priority, () => action());
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
            DispatcherQueuePriority priority, Action action)
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
            DispatcherQueuePriority priority, Action action)
        {
            EnsureDispatch();
            _dispatch(action, false, priority);
        }
    }
}
