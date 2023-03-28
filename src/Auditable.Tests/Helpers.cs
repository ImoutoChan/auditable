using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json.Linq;
using ObjectsComparer;
using PowerAssert;

namespace Auditable.Tests
{
    public static class Helpers
    {
        public static string AuditId => "audit-id";
        public static string Host => Environment.MachineName;
        public static string Application => Assembly.GetEntryAssembly().FullName;

        public static void Compare<T>(T left, T right, Action<Comparer<T>> setup = null)
        {
            var comparer = new Comparer<T>();
            setup?.Invoke(comparer);

            var different = comparer.Compare(left, right, out var differences);
            if (differences.Any()) throw new DifferentException(differences);
        }

        public static bool AssertDelta<T, TValue>(
            JToken delta,
            Expression<Func<T, TValue>> property,
            object before,
            object after)
        {
            var path = FindMemberVisitor.GetMember(property);

            var entry = delta.SelectToken($"$.{path}");

            PAssert.IsTrue(() => Equals(entry[0].Value<TValue>(), before));
            PAssert.IsTrue(() => Equals(entry[1].Value<TValue>(), after));

            return true;
        }
    }
}