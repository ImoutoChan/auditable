using System;
using Auditable.Configuration;
using Auditable.Infrastructure;
using Auditable.Tests.Models.Simple;
using Machine.Specifications;
using Microsoft.Extensions.DependencyInjection;
using PowerAssert;

namespace Auditable.Tests.Core
{
    [Subject("auditable")]
    public class When_the_scope_name_is_set
    {
        private static IAuditableContext _subject;
        private static TestWriter _writer;

        private Establish context = () =>
        {
            SystemDateTime.SetDateTime(() => new DateTime(1980, 01, 02, 10, 30, 15, DateTimeKind.Utc));
            var container = ApplicationContainer.Build(configureAuditable: services => services.AddAuditable());
            var scope = container.CreateScope();

            var auditable = scope.ServiceProvider.GetService<IAuditable>();
            _writer = scope.ServiceProvider.GetService<TestWriter>();

            //could have loaded it from the DB
            var person = new Person();
            person.Id = "123";
            person.Age = 38;
            person.Name = "Dave";

            //register object
            _subject = auditable.CreateContext("Person.Modified", person);
            person.Age = 21;
        };

        private Because of = () => _subject.WriteLog().Await();

        private It should_log_an_entry_with_the_action_name_set = () =>
            PAssert.IsTrue(() => _writer.First.Deserialize().Action == "Person.Modified");
    }
}