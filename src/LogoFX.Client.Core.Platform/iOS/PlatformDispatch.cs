using System.Threading.Tasks;
using Foundation;
using UIKit;

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
            UIApplication.SharedApplication.InvokeOnMainThread(action);
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
            return NSThread.IsMain;
        }        
       
        private Task OnUIThreadAsync(Action action)
        {
            var completionSource = new TaskCompletionSource<bool>();

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
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
            });

            return completionSource.Task;
        }       
    }
}
