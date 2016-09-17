using FrameworkInternalResource;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class EnvironmentResourceTests
    {
        const string EnUs = "en-US";
        const string PtBr = "pt-BR";
        const string AggregateException_ctor_DefaultMessage = "AggregateException_ctor_DefaultMessage";
        const string AggregateException_ctor_InnerExceptionNull = "AggregateException_ctor_InnerExceptionNull";
        const string AggregateException_DeserializationFailure = "AggregateException_DeserializationFailure";
        const string AggregateException_ToString = "AggregateException_ToString";

        [TestCase(AggregateException_ctor_DefaultMessage, EnUs, Result = "One or more errors occurred.")]
        [TestCase(AggregateException_ctor_DefaultMessage, PtBr, Result = "Um ou mais erros.")]
        [TestCase(AggregateException_ctor_InnerExceptionNull, EnUs, Result = "An element of innerExceptions was null.")]
        [TestCase(AggregateException_ctor_InnerExceptionNull, PtBr, Result = "Um elemento de innerExceptions era nulo.")]
        [TestCase(AggregateException_DeserializationFailure, EnUs, Result = "The serialization stream contains no inner exceptions.")]
        [TestCase(AggregateException_DeserializationFailure, PtBr, Result = "O fluxo da serialização não contém exceções internas.")]
        [TestCase(AggregateException_ToString, EnUs, Result = "{0}{1}---> (Inner Exception #{2}) {3}{4}{5}")]
        [TestCase(AggregateException_ToString, PtBr, Result = "{0}{1}---> (Exceção Interna N° {2}) {3}{4}{5}")]
        public string GetStringTest(string key, string cultureName)
        {
            return EnvironmentResource.GetString(key, cultureName);
        }
    }
}
