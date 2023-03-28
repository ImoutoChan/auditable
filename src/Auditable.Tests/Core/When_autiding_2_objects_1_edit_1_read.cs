using System;
using System.Collections.Generic;
using Auditable.Configuration;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Auditable.Tests.Models.Simple;
using Machine.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Environment = Auditable.Collectors.Environment.Environment;

namespace Auditable.Tests.Core
{
    [Subject("auditable")]
    public class When_autiding_2_objects_1_edit_1_read
    {
        private static IAuditableContext _subject;
        private static TestWriter _writer;

        private Establish context = () =>
        {
            SystemDateTime.SetDateTime(() => new DateTime(1980, 01, 02, 10, 3, 15, DateTimeKind.Utc));
            var container = ApplicationContainer.Build(configureAuditable: services => services.AddAuditable());
            var scope = container.CreateScope();
            var auditable = scope.ServiceProvider.GetService<IAuditable>();
            _writer = scope.ServiceProvider.GetService<TestWriter>();

            var person = new Person();
            var person2 = new Person
            {
                Id = "abc",
                Age = 21,
                Name = "Alexia"
            };

            _subject = auditable.CreateContext("Some.Action", person, person2);
            person.Id = "123";
            person.Age = 38;
            person.Name = "Dave";
        };

        private Because of = () => _subject.WriteLog().Await();

        private It should_add_a_single_log_entry_with_2_targets = () =>
            Helpers.Compare(_writer.First.Deserialize(), _expeted, comparer => comparer.IgnoreMember("Delta"));

        private static AuditableEntry _expeted => new()
        {
            Id = Helpers.AuditId,
            Action = "Some.Action",
            DateTime = SystemDateTime.UtcNow,
            Environment = new Environment
            {
                Host = Helpers.Host,
                Application = Helpers.Application
            },
            Initiator = null,
            Request = null,
            Targets = new List<AuditableTarget>
            {
                new()
                {
                    Id = "abc",
                    Audit = AuditType.Read,
                    Style = ActionStyle.Observed,
                    Type = typeof(Person).FullName
                },
                new()
                {
                    Id = null,
                    Audit = AuditType.Modified,
                    Style = ActionStyle.Observed,
                    Type = typeof(Person).FullName
                }
            }
        };
    }
}