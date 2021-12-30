Common Helper for Unit Test.

 To support a unit test of projects this lib contain a more function for 
helper and tools with scope to wrap Strategy for test productus to release.

//****************************//
List of Utils integrate.:

    ContextIOSystem.CheckKeywordsExist :: "Check in file phrase"

//****************************//
List of Extensions for Scopes.:

    ::Di and IoC:: (IocExtensions)

    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    RegisterAsImplementedInterfaces<>:: ( extend IServiceCollection )

            Add on IoC BuiltIn MS Di Microsoft.Extensions.DependencyInjection, this method for
            a helper method to register the type as providing all of its public interfaces. 
            This helper methods is manually to use in context also it is possible use a small 
            NuGet library (e.g. NetCore.AutoRegisterDi).
            ::Extension::
                --> void RegisterAsImplementedInterfaces<TService>
    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    RegisterModule<>                :: ( extend IServiceCollection )

            We all love to have modules to simplify registrations on our DI framework of choice.
            a helper method to register module for factory of class implementated with T concrete
            of inherited Module class present in this tool with virtual Load(..services..).            
            ::Extension::
                --> IServiceCollection RegisterModule<T> where T : Module
    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    ::Moq::

    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    ReturnsInOrder                  :: ( extend ISetup<T, TResult> )

            Return in order Sequencer list of Mock Objects values used for Results to 
            return in context of mocked context.
            ::Extension::
                --> void ReturnsInOrder<T, TResult>  where T : class
    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    Capture                         :: ( extend ICallback<TMock, Task> )

            Captures one or two arguments for the current setup Mocked with scope to use
            in construnct of callback to manage values.
            ::Extension::
                --> IReturnsThrows<TMock, Task> Capture<TMock, T1>  where TMock : class
    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    NotifyPropertyChanged           :: ( extend Mock<T> )

            Simplify PropertyChanged on Moq Setup of invoker delegates.
            ::Extension::
                --> IReturnsResult<T> NotifyPropertyChanged<T, TResult>
                ..See a example to Help for see a difference from..
                ::When not extension::
                    --> sampleModel.Setup(m => m.Value).Returns("NewValue");
                    --> sampleModel.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("Value"));
                ::With this extension::
                    --> sampleModel.NotifyPropertyChanged(m => m.Value, "NewValue");
    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    SetupWithVerification           :: ( extend Mock<TMocked> )

            Extension methods (and two classes) for creating setups with a matching verification, 
            this makes you write less code and avoid errors such as mismatching expressions between 
            setup and verification.
            ::Extension::
                --> Verification<TMocked> SetupWithVerification<TMocked> where TMocked : class
                ..See a example to Help for see a difference from..
                var mock = new Mock<VerificationStunt>();
                mock.SetupWithVerification(m => m.DoWork(), Times.Once,it => it.Callback(() => configured = true));
                mock.Object.DoWork();
                configured.Should().BeTrue();
                ::or
                --> Verification<TMocked, TResult> SetupWithVerification<TMocked, TResult>
            ** For Class to use with this extension check SetupWithVerification
    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    
//****************************//
List of Tools.:

    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    Abstract MoqTestDataBuilder     ::  ( class abstract ) ~Moq(Setup)/Builtin MS Container~

            Support for a free implementation of Moq Setup with context a build 
            object. To implement a Test Builder on Class complex with subclass
            ::use this class base abstract in this mode::
                --> public class ConcreteClassDataBuilder : MoqTestDataBuilder<ChildClass, ConcreteClassDataBuilder>
    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    Static Its                      :: ( class helper ) ~Moq/FluentAssertions~
 
            Contains helper methods that combine fuctionality of Moq and FluentAssertions
            to make it easier to work with complex input parameters in mocks.
            ::use this tool helper static in this mode::
                --> TValue Its.EquivalentTo<TValue>(TValue expected, ..predicate for config..)
                OR
                --> bool Its.AreEquivalent<TValue>(TValue actual, TValue expected, ..predicate for config..)
            to Matches any value that is equivalent to <paramref name="expected"
    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
    Class Concept                   :: ( class helper ) ~Moq(Setup)/Builtin MS Container~

            Contains a class Concept we to provides easy mocking for a given unit, 
            on resulting in an Ms Builtin container .net used to resolve the unit that 
            should be tested.
            ::use this tool helper class implmentation in this mode::
                --> this.concept.Mock<IOtherStringService>()
                        .Setup(oss => oss.GetString())
                        .Returns("World");
                    using (var container = this.concept.Build()){.....}
    - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

//****************************//
Notes.:

    ** For Extension RegisterAsImplementedInterfaces see..: "" https://alex-klaus.com/webapi-proj-without-autofac/ ""



    
