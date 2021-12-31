using System;
//
using NUnit.Framework;
using FluentAssertions;
using Moq;
using AutoFixture;
//

using HelperAndToolsForUT.Helper.Test.FluentAssertions.Tools;
using HelperAndToolsForUT.Helper.Extensions.MoqExtensions;

namespace HelperAndToolsForUT.Helper.Test.FluentAssertions
{


    [TestFixture]
    public class ItsTests
    {
        private Fixture _fixture;
        private Mock<IInterface> _mock;

        [SetUp]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _mock = new Mock<IInterface>();
        }

        [Test]
        public void EquivalentTo_Matches_Same_Complex_Types()
        {
            var complexType = _fixture.Create<ComplexType>();

            _mock.Object.DoSomething(complexType);

            _mock.Verify(m => m.DoSomething(Its.EquivalentTo(complexType)));
        }

        [Test]
        public void EquivalentTo_Matches_Two_Different_Complex_Types_With_Same_Data()
        {
            var complexType = _fixture.Create<ComplexType>();
            var expectedComplexType = complexType.Copy();

            _mock.Object.DoSomething(complexType);

            _mock.Verify(m => m.DoSomething(Its.EquivalentTo(expectedComplexType)));
        }

        [Test]
        public void EquivalentTo_Does_Not_Match_Two_Complex_Types_If_Child_Property_Has_Different_Value()
        {
            var complexType = _fixture.Create<ComplexType>();

            var expectedComplexType = complexType.Copy();
            expectedComplexType.ComplexTypeProperty.IntProperty++;

            _mock.Object.DoSomething(complexType);

            Action verify = () => _mock.Verify(m => m.DoSomething(Its.EquivalentTo(expectedComplexType)));
            verify.Should().Throw<MockException>();
        }

        [Test]
        public void EquivalentTo_Matches_Two_Complex_Types_If_Child_Property_Has_Different_Value_But_Its_Ignored()
        {
            var complexType = _fixture.Create<ComplexType>();

            var expectedComplexType = complexType.Copy();
            expectedComplexType.ComplexTypeProperty.IntProperty++;

            _mock.Object.DoSomething(complexType);

            _mock.Verify(m => m.DoSomething(Its.EquivalentTo(
                expectedComplexType,
                options => options.Excluding(c => c.ComplexTypeProperty.IntProperty)
            )));
        }

        [Test]
        public void EquivalentTo_Matches_If_Actual_And_Expected_Are_Null()
        {
            _mock.Object.DoSomething(null);

            _mock.Verify(m => m.DoSomething(Its.EquivalentTo<ComplexType>(null)));
        }

        [Test]
        public void EquivalentTo_Does_Not_Match_If_Actual_Object_Has_Value_And_Expected_Object_Is_Null()
        {
            var complexType = _fixture.Create<ComplexType>();

            _mock.Object.DoSomething(complexType);

            Action verify = () => _mock.Verify(m => m.DoSomething(Its.EquivalentTo<ComplexType>(null)));
            verify.Should().Throw<MockException>();
        }

        [Test]
        public void EquivalentTo_Does_Not_Match_If_Actual_Object_Is_Null_And_Expected_Object_Has_Value()
        {
            var expectedComplexType = _fixture.Create<ComplexType>();

            _mock.Object.DoSomething(null);

            Action verify = () => _mock.Verify(m => m.DoSomething(Its.EquivalentTo(expectedComplexType)));
            verify.Should().Throw<MockException>();
        }
    }
}