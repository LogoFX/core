 // ReSharper disable once CheckNamespace
namespace System.Threading
{
    /// <summary>
    /// Represents UI-thread dispatcher
    /// </summary>
    public interface IDispatch
    {
        /// <summary>
        /// Executes the action on the UI thread asynchronously
        /// </summary>
        /// <param name="action">Action</param>
        void BeginOnUiThread(Action action);      

        /// <summary>
        /// Executes the action on the UI thread
        /// </summary>
        /// <param name="action">Action</param>
        void OnUiThread(Action action);        

        /// <summary>
        /// Initializes the dispatcher
        /// </summary>
        void InitializeDispatch();
    }
}