// ReSharper disable once CheckNamespace
namespace System.Threading
{
    /// <summary>
    /// Ambient Context for <see cref="IDispatch"/>
    /// </summary>
    public static class Dispatch
    {
        /// <summary>
        /// Gets or sets the current dispatcher.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public static IDispatch Current { get; set; } = CreateDefaultDispatch();
        
        private static IDispatch CreateDefaultDispatch() => new DefaultDispatch();        
    }
}
