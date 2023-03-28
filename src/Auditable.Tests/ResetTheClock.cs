using Auditable.Infrastructure;
using Machine.Specifications;

namespace Auditable.Tests
{
    public class ResetTheClock : ICleanupAfterEveryContextInAssembly
    {
        public void AfterContextCleanup()
        {
            SystemDateTime.Reset();
        }
    }
}