using Moq;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace HelperAndToolsForUT.Helper.Abstraction.MOQ
{

    /// <summary>
    ///     Base to Implement Test Builder on Class complex with subclass
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// namespace Project.MoqClass.Tests
    /// {
    ///      using NUnit.Framework;
    ///     
    ///      <summary>
    ///         Non-sealed target type.
    ///      </summary>
    ///      public class User
    ///      {
    ///          public string LastName { get; set; }
    ///      
    ///          public string FirstName { get; set; }
    ///      }
    ///      
    ///      /// <summary>
    ///      /// Target type test data builder.
    ///      /// </summary>
    ///      public class UserTestDataBuilder : MoqTestDataBuilder<User, UserTestDataBuilder>
    ///      {
    ///          public UserTestDataBuilder WithLastName(string lastName)
    ///          {
    ///              return this.RegisterValueForProperty(x => x.LastName, lastName);
    ///          }
    ///      
    ///          public UserTestDataBuilder WithFirstName(string firstName)
    ///          {
    ///              return this.RegisterValueForProperty(x => x.FirstName, firstName);
    ///          }
    ///      }
    ///      
    ///      [TestFixture]
    ///      public class UserTestDataBuilderTests
    ///      {
    ///          [Test]
    ///          public void ComplexMockedObject_Success()
    ///          {
    ///              // arrange
    ///              const string targetLastName = "LastName";
    ///      
    ///              const string targetFirstName = "FirstName";
    ///      
    ///              // act
    ///              var user = new UserTestDataBuilder()
    ///                  .WithLastName(targetLastName)
    ///                  .WithFirstName(targetFirstName)
    ///                  .Build();
    ///      
    ///              // assert
    ///              Assert.IsNotNull(user);
    ///              Assert.AreEqual(targetLastName, user.LastName);
    ///              Assert.AreEqual(targetFirstName, user.FirstName);
    ///          }
    ///      }
    /// }
    ///     ]]></code>
    /// </example>
    /// <typeparam name="TObject"></typeparam>
    /// <typeparam name="TBuilder"></typeparam>
    public abstract class MoqTestDataBuilder<TObject, TBuilder>
        where TObject : class
        where TBuilder : MoqTestDataBuilder<TObject, TBuilder>
    {
        private readonly ParameterExpression target = Expression.Parameter(typeof(TObject));

        private BinaryExpression setup;

        /// <summary>
        ///     Registering in builder for build a series of property of setup MOQ
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected TBuilder RegisterValueForProperty<TValue>(Expression<Func<TObject, TValue>> expression, TValue value)
        {
            if (!((expression.Body as MemberExpression)?.Member is PropertyInfo targetProperty))
            {
                throw new ArgumentOutOfRangeException(nameof(expression), "Expression doesn't extract property of target type.");
            }

            var result = Expression.Equal(
                Expression.Property(this.target, targetProperty),
                Expression.Constant(value));

            return this.UpdateSetup(result);
        }

        /// <summary>
        ///     Register a Flag in Setup builder
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected TBuilder RegisterFlag(Expression<Func<TObject, bool>> expression)
        {
            return this.RegisterValueForProperty(expression, true);
        }

        /// <summary>
        ///     Update with expression a setupe Mock
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected TBuilder UpdateSetup(BinaryExpression expression)
        {
            this.setup = this.IsDefaultSetup()
                ? expression
                : Expression.AndAlso(this.setup, expression);

            return (TBuilder)this;
        }

        /// <summary>
        ///     Is a Default Setup of Mock
        /// </summary>
        /// <returns></returns>
        protected bool IsDefaultSetup()
        {
            return this.setup is null;
        }

        /// <summary>
        ///     Builder of this builder context.
        /// </summary>
        /// <returns></returns>
        public TObject Build()
        {
            return this.IsDefaultSetup()
                ? Mock.Of<TObject>()
                : Mock.Of<TObject>(Expression.Lambda<Func<TObject, bool>>(this.setup, this.target));
        }
    }
}
