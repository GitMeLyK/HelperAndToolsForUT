using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperAndToolsForUT.Helper.SetupWithVerification
{
    public abstract class VerificationBase<TMocked> : IDisposable where TMocked : class
    {
        public Mock<TMocked> Mock { get; set; }

        protected VerificationBase(Mock<TMocked> mock = null)
        {
            Mock = mock ?? new Mock<TMocked>();
        }

        public abstract void Verify();

        public void Dispose() => Verify();
    }
}
