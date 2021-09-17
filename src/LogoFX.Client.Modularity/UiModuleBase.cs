namespace LogoFX.Client.Modularity
{
    /// <summary>
    /// Represents ui module without root view model.
    /// Used to clear the root view model in the extending module.
    /// </summary>
    public abstract class UiModuleBase
    {
        /// <summary>
        /// Clears the root view model.
        /// </summary>
        public abstract void ClearRoot();
    }    
}