using System;
using System.Collections.Generic;
using System.Net.Http;
using Auditable.AspNetCore.Tests.Infrastructure;
using Auditable.Collectors.Initiator;
using Auditable.Collectors.Request;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Auditable.Tests;
using Auditable.Tests.Models.Simple;
using Auditable.Writers;
using Machine.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Environment = Auditable.Collectors.Environment.Environment;

namespace Auditable.AspNetCore.Tests
{
    [Subject("auditable")]
    public class When_creating_a_log_entry_with_an_authorized_user_and_request_id
    {
        private static CustomWebApplicationFactory<Startup> _factory;
        private static TestWriter _writer;
        private static HttpClient _client;


        private Cleanup after = () => _factory.Dispose();


        private Establish context = () =>
        {
            SystemDateTime.SetDateTime(() => new DateTime(1980, 01, 02, 10, 3, 15, DateTimeKind.Utc));
            _writer = new TestWriter();
            _factory = new CustomWebApplicationFactory<Startup>(services =>
            {
                services.AddSingleton<IWriter>(_writer);
            });


            _client = _factory.CreateClient();
            //"traceparent: 00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01"
            _client.DefaultRequestHeaders.Add("traceparent",
                new[] { "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01" });
        };

        private Because of = () => _client.GetAsync("/test").Await();

        private It should_add_the_expected_log_entry = () =>
            Helpers.Compare(_writer.First.Deserialize(), _expeted, comparer =>
            {
                comparer.IgnoreMember("Delta");
                comparer.IgnoreMember("SpanId");
            });

        private static AuditableEntry _expeted => new()
        {
            Id = Helpers.AuditId,
            Action = "test.get",
            DateTime = SystemDateTime.UtcNow,
            Environment = new Environment
            {
                Host = Helpers.Host,
                Application = Helpers.Application
            },
            Initiator = new Initiator
            {
                Id = "abc-123",
                Name = "dave"
            },
            Request = new RequestContext
            {
                ParentId = "00f067aa0ba902b7",
                TraceId = "4bf92f3577b34da6a3ce929d0e0e4736"
            },
            Targets = new List<AuditableTarget>
            {
                new()
                {
                    Id = "123",
                    Audit = AuditType.Read,
                    Style = ActionStyle.Explicit,
                    Type = typeof(Person).FullName
                }
            }
        };
    }
}