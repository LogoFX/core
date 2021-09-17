using System.Threading.Tasks;
using Android.App;

// ReSharper disable once CheckNamespace
namespace System.Threading
{
    /// <summary>
    /// Platform-specific dispatcher
    /// </summary> 
    public class PlatformDispatch : IDispatch
    {
        /// <inheritdoc /> 
        public void BeginOnUiThread(Action action)
        {
            Application.SynchronizationContext.Post(s => action(), null);
        }

        /// <inheritdoc /> 
        public void OnUiThread(Action action)
        {
            if (CheckAccess())
                action();
            else
                OnUIThreadAsync(action).Wait();
        }

        /// <inheritdoc /> 
        public void InitializeDispatch()
        {}

        private bool CheckAccess()
        {
            return SynchronizationContext.Current != null;
        }

        private Task OnUIThreadAsync(Action action)
        {
            var completionSource = new TaskCompletionSource<bool>();

            Application.SynchronizationContext.Post(s => {
                try
                {
                    action();
                    completionSource.SetResult(true);
                }
                catch (TaskCanceledException)
                {
                    completionSource.SetCanceled();
                }
                catch (Exception ex)
                {
                    completionSource.SetException(ex);
                }
            }, null);

            return completionSource.Task;
        }
    }
}
