using System.Windows.Threading;
using LogoFX.Client.Core;

// ReSharper disable once CheckNamespace
namespace System.Threading
{    
    /// <summary>
    /// Platform-specific dispatcher
    /// </summary>
    public class PlatformDispatch : IDispatch
    {
        private Action<Action, bool, DispatcherPriority> _dispatch;        

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
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (dispatcher == null)
                throw new InvalidOperationException("Dispatch is not initialized correctly");
            _dispatch = (action, @async, priority) =>
            {
                if (!@async && dispatcher.CheckAccess())
                {
                    action();
                }               
                else
                {
                    dispatcher.BeginInvoke(action, priority);
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
            DispatcherPriority priority, Action action)
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
            DispatcherPriority priority, Action action)
        {
            EnsureDispatch();
            _dispatch(action, false, priority);
        }
    }
}
