using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
//
using Moq;
using Moq.Language.Flow;

namespace HelperAndToolsForUT.Helper.MoqExtensions
{

    public static partial class MoqOrderAndNotifyExtensions
    {
        /// <summary>
        ///     Return in order Sequencer of TResult objects Results
        /// </summary>
        /// <example>
        ///     var mock = new Mock<ISomeInterface>();
        ///     mock.Setup(r => r.GetNext())
        ///     .ReturnsInOrder(1, 2, new InvalidOperationException());
        ///     
        ///     Console.WriteLine(mock.Object.GetNext());
        ///     Console.WriteLine(mock.Object.GetNext());
        ///     Console.WriteLine(mock.Object.GetNext()); // Throws InvalidOperationException
        /// </example>
        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup,
            params TResult[] results) where T : class
        {
            setup.Returns(new Queue<TResult>(results).Dequeue);
        }

        /// <summary>
        ///     Return in order Sequencer of object Results
        /// </summary>
        /// <example>
        ///     var mock = new Mock<ISomeInterface>();
        ///     mock.Setup(r => r.GetNext())
        ///     .ReturnsInOrder(1, 2, new InvalidOperationException());
        ///     
        ///     Console.WriteLine(mock.Object.GetNext());
        ///     Console.WriteLine(mock.Object.GetNext());
        ///     Console.WriteLine(mock.Object.GetNext()); // Throws InvalidOperationException
        /// </example>
        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup, params object[] results) where T : class
        {
            var queue = new Queue(results);
            setup.Returns(() =>
            {
                var result = queue.Dequeue();
                if (result is Exception)
                {
                    throw result as Exception;
                }
                return (TResult)result;
            });
        }

        // 

        /// <summary>
        ///     Simplify PropertyChanged on Moq Setup of invoker delegates.
        /// </summary>
        /// <example>
        ///     public interface ISampleModel : INotifyPropertyChanged {
        ///         string Value { get; set; }
        ///     }
        ///     
        ///     public class SampleModel : ISampleModel
        ///     {
        ///         public event PropertyChangedEventHandler PropertyChanged;
        ///         private string _value;
        ///     
        ///         public string Value
        ///         {
        ///             get { return _value; }
        ///             set
        ///             {
        ///                 if (!Equals(_value, value))
        ///                 {
        ///                     _value = value;
        ///                     PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        ///                 }
        ///             }
        ///     
        ///         }
        ///     }
        ///     
        ///     :: TEST
        ///     [Fact]
        ///     public void WhenDoNotUseExtensions()
        ///     {
        ///         var sampleModel = new Mock<ISampleModel>();
        ///         var actual = new SampleViewModel(sampleModel.Object);
        ///     
        ///         sampleModel.Setup(m => m.Value).Returns("NewValue");
        ///         sampleModel.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("Value"));
        ///     
        ///         Assert.Equal("NewValue", actual.ViewModelValue);
        ///     }
        ///     
        ///     [Fact]
        ///     public void WhenUseExtensions()
        ///     {
        ///         var sampleModel = new Mock<ISampleModel>();
        ///         var actual = new SampleViewModel(sampleModel.Object);
        ///     
        ///         sampleModel.NotifyPropertyChanged(m => m.Value, "NewValue");
        ///     
        ///         Assert.Equal("NewValue", actual.ViewModelValue);
        ///     }        
        ///     
        ///     ::When not extension::
        ///     --> sampleModel.Setup(m => m.Value).Returns("NewValue");
        ///     --> sampleModel.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("Value"));
        /// 
        ///     ::With this extension::
        ///     --> sampleModel.NotifyPropertyChanged(m => m.Value, "NewValue");
        /// 
        /// </example>
        /// <returns></returns>
        public static IReturnsResult<T> NotifyPropertyChanged<T, TResult>(this Mock<T> mock, Expression<Func<T, TResult>> expression, TResult setupValue) where T : class, INotifyPropertyChanged
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null) throw new ArgumentException("expression.Body is not MemberExpression");

            var returnResult = mock.Setup(expression).Returns(setupValue);

            mock.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs(memberExpression.Member.Name));

            return returnResult;
        }

    }
}
