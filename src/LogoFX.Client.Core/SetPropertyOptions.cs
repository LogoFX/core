//note: I don't know who is responsible for writing out the most of this excellent stuff
//note: If you feel you are somehow involved and not mentioned in credits - let me know
using System;

namespace LogoFX.Client.Core
{
    /// <summary>
    /// Set property options. Use this to inject functionality on property value update.
    /// </summary>
    public class SetPropertyOptions
    {
        /// <summary>
        /// Invoked before a property value is updated.
        /// </summary>
        public Action BeforeValueUpdate { get; set; }

        /// <summary>
        /// Invoked after a property value is updated.
        /// </summary>
        public Action AfterValueUpdate { get; set; }

        /// <summary>
        /// Allows injecting custom value update invocation method.
        /// </summary>
        public Action<Action> CustomActionInvocation { get; set; }
    }
}
