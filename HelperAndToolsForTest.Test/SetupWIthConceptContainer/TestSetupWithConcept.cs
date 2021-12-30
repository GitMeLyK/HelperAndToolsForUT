using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

using HelperAndToolsForUT.Helper.MoqExtensions;
using HelperAndToolsForUT.Helper.DiExtensions;

namespace HelperAndToolsForUT.Helper.SetupWIthConcept.Test
{

    #region ### SERVICE ###

    interface IStringService
    {
        string GetString();
    }

    interface IOtherStringService : IStringService
    {
        //
    }

    class StringService : IStringService, IOtherStringService
    {
        public string GetString()
        {
            return "Hello";
        }
    }

    #endregion

    #region ### FIXTURE ###

    interface IFixture
    {
        string GetCombinedStrings();
    }

    interface ICaptureFixture
    {
        void DoSomething(string key);
        void DoSomething(string key, int value);
        Task DoSomethingAsync(string key);
        Task DoSomethingAsync(string key, int value);
    }

    class Fixture : IFixture
    {
        private readonly IStringService stringService;
        private readonly IOtherStringService otherStringService;

        public Fixture(IStringService stringService
            , IOtherStringService otherStringService
            )
        {
            this.stringService = stringService;
            this.otherStringService = otherStringService;
        }

        public string GetCombinedStrings()
        {
            return $"{this.stringService.GetString()} {this.otherStringService.GetString()}";
        }
    }

    #endregion


    #region ### CONCEPT TEST ###

    [TestFixture]
    public class ConceptTests
    {
        private Concept concept;

        /* AutoFac
        [SetUp]
        public void Initialize()
        {
            this.concept = new Concept();

            this.concept.Builder.RegisterType<Fixture>()
                .As<IFixture>()
                .SingleInstance();

            this.concept.Builder.RegisterType<StringService>()
                .As<IStringService>()
                .As<IOtherStringService>()
                .SingleInstance();
        }
        */

        [SetUp] // Ioc MS
        public void Initialize()
        {
            this.concept = new Concept();

            this.concept.Builder.AddScoped<IFixture, Fixture>();

            // I have used helper extension to simplify
            this.concept.Builder.RegisterAsImplementedInterfaces<StringService>( ServiceLifetime.Singleton );

        }

        [Test]
        public void ShouldDisplayNormalBehaviorWhenNotMocked()
        {
            using (var container = this.concept.Build())
            {
                // AutoFac
                // Assert.AreEqual("Hello Hello", container.Resolve<IFixture>().GetCombinedStrings());

                // Container MS Ioc Builtin
                Assert.AreEqual("Hello Hello", container.GetService<IFixture>().GetCombinedStrings());
            }
        }

        [Test]
        public void ShouldBeAbleToStubOutServices()
        {
            this.concept.Mock<IOtherStringService>()
                .Setup(oss => oss.GetString())
                .Returns("World");

            using (var container = this.concept.Build())
            {
                // AutoFac
                //var fixture = container.Resolve<IFixture>();

                // Container MS Ioc Builtin
                var fixture = container.GetService<IFixture>();

                // AutoFac
                //Assert.AreEqual("Hello World", container.Resolve<IFixture>().GetCombinedStrings());

                // Container MS Ioc Builtin
                Assert.AreEqual("Hello World", container.GetService<IFixture>().GetCombinedStrings());

            }
        }

        [Test]
        public void ShouldStubOutServicesAsSingletons()
        {
            Assert.AreSame(this.concept.Mock<IStringService>(), this.concept.Mock<IStringService>());

            using (var container = this.concept.Build())
            {
                // AutoFac
                // Assert.AreSame(container.Resolve<IStringService>(), container.Resolve<IStringService>());
                // Assert.AreSame(this.concept.Mock<IStringService>().Object, container.Resolve<IStringService>());

                // Container MS Ioc Builtin
                Assert.AreSame(container.GetService<IStringService>(), container.GetService<IStringService>());
                Assert.AreSame(this.concept.Mock<IStringService>().Object, container.GetService<IStringService>());

            }
        }
    }

    #endregion


    #region ### TEXT ON EXTENSION ASYNC ###

    [TestFixture]
    public class MockAsyncCaptureExtensionsTests
    {
        [Test]
        public async Task ShouldBeAbleToCaptureSingleParameterMethodArguments()
        {
            var expected = new[]
            {
                "first",
                "second",
                "third",
            };

            var mock = new Mock<ICaptureFixture>();

            var results = new List<string>();
            mock
                .Setup(f => f.DoSomethingAsync(It.IsAny<string>()))
                .Capture(results)
                .Returns(Task.CompletedTask);

            foreach (var key in expected)
            {
                await mock.Object.DoSomethingAsync(key);
            }

            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public async Task ShouldBeAbleToCaptureTwoParameterMethodArgumentsAsTuples()
        {
            var expected = new[]
            {
                new Tuple<string, int>("first", 42),
                new Tuple<string, int>("second", 43),
                new Tuple<string, int>("third", 44),
            };

            var mock = new Mock<ICaptureFixture>();

            var results = new List<Tuple<string, int>>();
            mock
                .Setup(f => f.DoSomethingAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Capture(results)
                .Returns(Task.CompletedTask);

            foreach (var tuple in expected)
            {
                await mock.Object.DoSomethingAsync(tuple.Item1, tuple.Item2);
            }

            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void ShouldBeAbleToCaptureTwoParameterMethodArgumentsAsLists()
        {
            var expectedStrings = new[]
            {
                "first",
                "second",
                "third",
            };

            var expectedInts = new[]
            {
                42,
                43,
                44
            };

            var mock = new Mock<ICaptureFixture>();

            var stringResults = new List<string>();
            var intResults = new List<int>();
            mock
                .Setup(f => f.DoSomethingAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Capture(stringResults, intResults);

            for (var i = 0; i < expectedStrings.Length; i++)
            {
                mock.Object.DoSomethingAsync(expectedStrings[i], expectedInts[i]);
            }

            CollectionAssert.AreEqual(expectedStrings, stringResults);
            CollectionAssert.AreEqual(expectedInts, intResults);
        }
    }

    #endregion

    #region ### TEST ON EXTENSION  SYNC ###

    [TestFixture]
    public class MockCaptureExtensionsTests
    {
        [Test]
        public void ShouldBeAbleToCaptureSingleParameterMethodArguments()
        {
            var expected = new[]
            {
                "first",
                "second",
                "third",
            };

            var mock = new Mock<ICaptureFixture>();

            var results = new List<string>();
            mock
                .Setup(f => f.DoSomething(It.IsAny<string>()))
                .Capture(results);

            foreach (var key in expected)
            {
                mock.Object.DoSomething(key);
            }

            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void ShouldBeAbleToCaptureTwoParameterMethodArgumentsAsTuples()
        {
            var expected = new[]
            {
                new Tuple<string, int>("first", 42),
                new Tuple<string, int>("second", 43),
                new Tuple<string, int>("third", 44),
            };

            var mock = new Mock<ICaptureFixture>();

            var results = new List<Tuple<string, int>>();
            mock
                .Setup(f => f.DoSomething(It.IsAny<string>(), It.IsAny<int>()))
                .Capture(results);

            foreach (var tuple in expected)
            {
                mock.Object.DoSomething(tuple.Item1, tuple.Item2);
            }

            CollectionAssert.AreEqual(expected, results);
        }

        [Test]
        public void ShouldBeAbleToCaptureTwoParameterMethodArgumentsAsLists()
        {
            var expectedStrings = new[]
            {
                "first",
                "second",
                "third",
            };

            var expectedInts = new[]
            {
                42,
                43,
                44
            };

            var mock = new Mock<ICaptureFixture>();

            var stringResults = new List<string>();
            var intResults = new List<int>();
            mock
                .Setup(f => f.DoSomething(It.IsAny<string>(), It.IsAny<int>()))
                .Capture(stringResults, intResults);

            for (var i = 0; i < expectedStrings.Length; i++)
            {
                mock.Object.DoSomething(expectedStrings[i], expectedInts[i]);
            }

            CollectionAssert.AreEqual(expectedStrings, stringResults);
            CollectionAssert.AreEqual(expectedInts, intResults);
        }
    }

    #endregion

}
