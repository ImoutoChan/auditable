using System;
using System.Collections.Generic;
using System.Linq;
using ObjectsComparer;

namespace Auditable.Tests
{
    public class DifferentException : Exception
    {
        public DifferentException(IEnumerable<Difference> differences)
            : base(string.Join(Environment.NewLine, differences.Select(x => x.ToString())))
        {
            Differences = differences;
        }

        public IEnumerable<Difference> Differences { get; }
    }
}