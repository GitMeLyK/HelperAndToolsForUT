using System;
using System.Linq.Expressions;

using Moq;

using HelperAndToolsForUT.Helper.Abstraction.MOQ;

namespace HelperAndToolsForUT.Helper.MOQ.SetupWithVerification
{

    /// <summary>
    ///     Class concrete implementated From VerificationBase<typeparamref name="TMocked"/>.
    /// </summary>
    /// <typeparam name="TMocked"></typeparam>
    public class Verification<TMocked> : VerificationBase<TMocked> where TMocked : class
    {
        private readonly Expression<Action<TMocked>> _expression;
        private readonly Func<Times> _times;

        /// <summary>
        ///     Class concrete implementated From VerificationBase<typeparamref name="TMocked"/>.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="times"></param>
        /// <param name="mock"></param>
        public Verification(Expression<Action<TMocked>> expression, Func<Times> times, Mock<TMocked> mock = null) : base(mock)
        {
            _expression = expression;
            _times = times;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Verify() => Mock.Verify(_expression, _times);
    }

    /// <summary>
    ///     Setup with Verification implementated with Class Verification implementate from abstract 
    ///     VerificationBase<typeparamref name="TMocked"/> and return a <typeparamref name="TReturn"/>
    /// </summary>
    /// <typeparam name="TMocked"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public class Verification<TMocked, TReturn> : VerificationBase<TMocked> where TMocked : class
    {
        private readonly Expression<Func<TMocked, TReturn>> _expression;
        private readonly Func<Times> _times;

        /// <summary>
        ///     Use for Setup and verification of MOQ setup
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="times"></param>
        /// <param name="mock"></param>
        public Verification(Expression<Func<TMocked, TReturn>> expression, Func<Times> times, Mock<TMocked> mock = null) : base(mock)
        {
            _expression = expression;
            _times = times;
        }

        /// <summary>
        ///     On derivated implement a verification case.
        /// </summary>
        public override void Verify() => Mock.Verify(_expression, _times);
    }
}
