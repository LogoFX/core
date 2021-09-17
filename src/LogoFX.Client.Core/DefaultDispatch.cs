using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Threading
{    
    /// <summary>
    /// Default implementation of <see cref="IDispatch"/>
    /// </summary>
    public class DefaultDispatch : IDispatch
    {
        /// <inheritdoc />
        public void BeginOnUiThread(Action action)
        {
            Task.Run(action);
        }

        /// <inheritdoc />
        public void OnUiThread(Action action)
        {
            action();
        }

        /// <inheritdoc />
        public void InitializeDispatch()
        {}
    }
}
