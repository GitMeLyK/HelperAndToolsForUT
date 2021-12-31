using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperAndToolsForUT.Helper.Abstraction.MOQ
{
    /// <summary>
    ///     Abstraction for context extension MOQ MoqExtensions.SetupWithVerification type
    /// </summary>
    /// <typeparam name="TMocked">MOQ context setup to use verification</typeparam>
    public abstract class VerificationBase<TMocked> : IDisposable where TMocked : class
    {
        /// <summary>
        ///     MOQ for context setup
        /// </summary>
        public Mock<TMocked> Mock { get; set; }

        /// <summary>
        ///     MOQ base
        /// </summary>
        /// <param name="mock"></param>
        protected VerificationBase(Mock<TMocked> mock = null)
        {
            Mock = mock ?? new Mock<TMocked>();
        }

        /// <summary>
        ///     To implement verification
        /// </summary>
        public abstract void Verify();

        /// <summary>
        ///     ~
        /// </summary>
        public void Dispose() => Verify();
    }
}
