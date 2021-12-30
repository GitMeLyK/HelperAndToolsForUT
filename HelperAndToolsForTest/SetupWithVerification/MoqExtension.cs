using System;
using System.Linq.Expressions;
using Moq;
using Moq.Language.Flow;
using HelperAndToolsForUT.Helper.SetupWithVerification;

namespace HelperAndToolsForUT.Helper.MoqExtensions
{

    /// <summary>
    ///     Extension methods (and two classes) for creating setups with a matching verification, 
    ///     this makes you write less code and avoid errors such as mismatching expressions between 
    ///     setup and verification
    /// </summary>
    public static partial class  MoqExtensions
    {

        /// <summary>
        ///     Extension methods (and two classes) for creating setups with a matching verification, 
        ///     this makes you write less code and avoid errors such as mismatching expressions between 
        ///     setup and verification
        /// </summary>
        /// <example>
        /// 
        ///  [Fact]
        ///  public void ActionSetupWithVerificationShouldConfigureMock()
        ///  {
        ///      bool configured = false;
        /// 
        ///      var mock = new Mock<VerificationStunt>();
        /// 
        ///      mock.SetupWithVerification(m => m.DoWork(), Times.Once,
        ///          it => it.Callback(() => configured = true));
        /// 
        ///      mock.Object.DoWork();
        /// 
        ///      configured.Should().BeTrue();
        ///  }
        /// 
        ///  [Fact]
        ///  public void FuncSetupWithVerificationShouldConfigureMock()
        ///  {
        ///      var mock = new Mock<VerificationStunt>();
        /// 
        ///      mock.SetupWithVerification(m => m.GetValue(), Times.Once, it => it.Returns(1));
        /// 
        ///      var result = mock.Object.GetValue();
        /// 
        ///      result.Should().Be(1);
        ///  }
        ///  
        /// </example>
        public static Verification<TMocked> SetupWithVerification<TMocked>(this Mock<TMocked> mock, Expression<Action<TMocked>> expression,
            Func<Times> times, Action<ISetup<TMocked>> configureSetup = null) where TMocked : class
        {
            var setup = mock.Setup(expression);

            configureSetup?.Invoke(setup);

            setup.Verifiable();

            return new Verification<TMocked>(expression, times, mock);
        }

        /// <summary>
        ///     Extension methods (and two classes) for creating setups with a matching verification, 
        ///     this makes you write less code and avoid errors such as mismatching expressions between 
        ///     setup and verification
        /// </summary>
        /// <example>
        /// 
        ///  [Fact]
        ///  public void ActionSetupWithVerificationShouldConfigureMock()
        ///  {
        ///      bool configured = false;
        /// 
        ///      var mock = new Mock<VerificationStunt>();
        /// 
        ///      mock.SetupWithVerification(m => m.DoWork(), Times.Once,
        ///          it => it.Callback(() => configured = true));
        /// 
        ///      mock.Object.DoWork();
        /// 
        ///      configured.Should().BeTrue();
        ///  }
        /// 
        ///  [Fact]
        ///  public void FuncSetupWithVerificationShouldConfigureMock()
        ///  {
        ///      var mock = new Mock<VerificationStunt>();
        /// 
        ///      mock.SetupWithVerification(m => m.GetValue(), Times.Once, it => it.Returns(1));
        /// 
        ///      var result = mock.Object.GetValue();
        /// 
        ///      result.Should().Be(1);
        ///  }
        ///  
        /// </example>
        public static Verification<TMocked, TResult> SetupWithVerification<TMocked, TResult>(this Mock<TMocked> mock,
            Expression<Func<TMocked, TResult>> expression, Func<Times> times, Action<ISetup<TMocked, TResult>> configureSetup = null) where TMocked : class
        {
            var setup = mock.Setup(expression);

            configureSetup?.Invoke(setup);

            setup.Verifiable();

            return new Verification<TMocked, TResult>(expression, times, mock);
        }
    }

}
