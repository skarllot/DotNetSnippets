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
        const string Task_Dispose_NotCompleted = "Task_Dispose_NotCompleted";
        const string SpinWait_SpinUntil_TimeoutWrong = "SpinWait_SpinUntil_TimeoutWrong";
        const string SpinWait_SpinUntil_ArgumentNull = "SpinWait_SpinUntil_ArgumentNull";

        [TestCase(AggregateException_ctor_DefaultMessage, EnUs, Result = "One or more errors occurred.")]
        [TestCase(AggregateException_ctor_DefaultMessage, PtBr, Result = "Um ou mais erros.")]
        [TestCase(AggregateException_ctor_InnerExceptionNull, EnUs, Result = "An element of innerExceptions was null.")]
        [TestCase(AggregateException_ctor_InnerExceptionNull, PtBr, Result = "Um elemento de innerExceptions era nulo.")]
        [TestCase(AggregateException_DeserializationFailure, EnUs, Result = "The serialization stream contains no inner exceptions.")]
        [TestCase(AggregateException_DeserializationFailure, PtBr, Result = "O fluxo da serialização não contém exceções internas.")]
        [TestCase(AggregateException_ToString, EnUs, Result = "{0}{1}---> (Inner Exception #{2}) {3}{4}{5}")]
        [TestCase(AggregateException_ToString, PtBr, Result = "{0}{1}---> (Exceção Interna N° {2}) {3}{4}{5}")]
        [TestCase(Task_Dispose_NotCompleted, EnUs, Result = "A task may only be disposed if it is in a completion state (RanToCompletion, Faulted or Canceled).")]
        [TestCase(Task_Dispose_NotCompleted, PtBr, Result = "Uma tarefa só pode ser descartada se estiver em estado de conclusão (RanToCompletion, Faulted ou Canceled).")]
        [TestCase(SpinWait_SpinUntil_TimeoutWrong, EnUs, Result = "The timeout must represent a value between -1 and Int32.MaxValue, inclusive.")]
        [TestCase(SpinWait_SpinUntil_TimeoutWrong, PtBr, Result = "O tempo limite deve representar um valor entre -1 e Int32.MaxValue, inclusive.")]
        [TestCase(SpinWait_SpinUntil_ArgumentNull, EnUs, Result = "The condition argument is null.")]
        [TestCase(SpinWait_SpinUntil_ArgumentNull, PtBr, Result = "O argumento de condição é nulo.")]
        public string GetStringTest(string key, string cultureName)
        {
            return EnvironmentResource.GetString(key, cultureName);
        }
    }
}
