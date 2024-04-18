using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using NUnit.Framework;

namespace ClassLibrary1
{
    [TestFixture]
    public class Test
    {
        private IContainer _pgContainer;

        [Test]
        public void ChangeUserLastCompanyTest()
        {
            var pgpassword = "1";
            if (_pgContainer == null)
            {
                var containerBuilder = new ContainerBuilder()
                    .WithImage("postgres:15.5")
                    .WithPortBinding(5432, true)
                    .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
                    .WithEnvironment("POSTGRES_PASSWORD", pgpassword);
                _pgContainer = containerBuilder
                    .Build();
                Console.WriteLine("starting postgres container");
                _pgContainer.StartAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }

            var pgPort = _pgContainer
                .GetMappedPublicPort(5432).ToString();

            var user = new BackOfficeUser();
            var bocontainer = new BackOfficeModelContainer(pgPort);
            bocontainer.Database.EnsureCreated();
            bocontainer.BackOfficeUsers.Add(user);
            bocontainer.SaveChanges();
            Stopwatch stopwatch = Stopwatch.StartNew();
            Parallel.For(0, 100, (i) =>
            {
                for (int j = 0; j < 10; j++)
                {
                    var bocontainer = new BackOfficeModelContainer(pgPort);
                    var tempuser = bocontainer.BackOfficeUsers.First();
                    tempuser.LastCompany = Guid.NewGuid();
                    bocontainer.BulkUpdate(new List<BackOfficeUser>() { tempuser });
                }
            });
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}