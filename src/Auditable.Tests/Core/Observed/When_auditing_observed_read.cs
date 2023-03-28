using System;
using System.Collections.Generic;
using Auditable.Configuration;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Auditable.Tests.Models.Simple;
using Machine.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Env = Auditable.Collectors.Environment.Environment;

namespace Auditable.Tests.Core.Observed
{
    [Subject("auditable")]
    public class When_auditing_observed_read
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

            var person = new Person
            {
                Id = "abc",
                Name = "Dave",
                Age = 24
            };

            _subject = auditable.CreateContext("Person.Read");
            _subject.WatchTargets(person);
        };

        private Because of = () => _subject.WriteLog().Await();

        private It should_have_a_target_with_audit_read_and_no_delta = () =>
            Helpers.Compare(_writer.First.Deserialize(), _expeted);

        private static AuditableEntry _expeted => new()
        {
            Id = Helpers.AuditId,
            Action = "Person.Read",
            DateTime = SystemDateTime.UtcNow,
            Environment = new Env
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
                }
            }
        };
    }
}