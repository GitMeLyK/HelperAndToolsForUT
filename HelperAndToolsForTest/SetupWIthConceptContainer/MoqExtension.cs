using Moq.Language;
using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelperAndToolsForUT.Helper.Extensions.MoqExtensions
{
    /// <summary>
    ///     Mock extensions for capturing arguments on a setup.
    /// </summary>
    public static class MockCaptureExtensions
    {
        /// <summary>
        ///     Captures a single argument for the current setup.
        /// </summary>
        /// <typeparam name="T1">The type of argument to capture.</typeparam>
        /// <param name="callback">The setup to capture arguments for.</param>
        /// <param name="results">The list to store captured arguments in.</param>
        /// <returns>The continuation of the setup.</returns>
        public static ICallbackResult Capture<T1>(this ICallback callback, List<T1> results)
        {
            return callback
                .Callback<T1>(p1 => results.Add(p1));
        }

        /// <summary>
        ///     Captures two arguments for the current setup.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument to capture.</typeparam>
        /// <typeparam name="T2">The type of the second argument to capture.</typeparam>
        /// <param name="callback">The setup to capture arguments for.</param>
        /// <param name="results">The list to store captured arguments in.</param>
        /// <returns>The continuation of the setup.</returns>
        public static ICallbackResult Capture<T1, T2>(this ICallback callback, List<Tuple<T1, T2>> results)
        {
            return callback.Callback<T1, T2>((p1, p2) => results.Add(new Tuple<T1, T2>(p1, p2)));
        }

        /// <summary>
        ///     Captures two arguments for the current setup.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument to capture.</typeparam>
        /// <typeparam name="T2">The type of the second argument to capture.</typeparam>
        /// <param name="callback">The setup to capture arguments for.</param>
        /// <param name="t1Results">The list to store the captured first arguments in.</param>
        /// <param name="t2Results">The list to store the captured second arguments in.</param>
        /// <returns>The continuation of the setup.</returns>
        public static ICallbackResult Capture<T1, T2>(this ICallback callback, List<T1> t1Results, List<T2> t2Results)
        {
            return callback.Callback<T1, T2>((p1, p2) =>
            {
                t1Results.Add(p1);
                t2Results.Add(p2);
            });
        }
    }

}
