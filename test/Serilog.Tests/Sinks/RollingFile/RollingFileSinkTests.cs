﻿using System;
using System.IO;
using NUnit.Framework;
using Serilog.Events;
using Serilog.Tests.Support;

namespace Serilog.Tests.Sinks.RollingFile
{
    [TestFixture]
    public class RollingFileSinkTests
    {
        [Test]
        public void LogEventsAreEmittedToTheFileNamedAccordingToTheEventTimeStamp()
        {
            TestRollingEventSequence(Some.LogEvent());
        }

        [Test]
        public void WhenTheDateChangesTheCorrectFileIsWritten()
        {
            var e1 = Some.LogEvent();
            var e2 = new LogEvent(e1.TimeStamp.AddDays(1), LogEventLevel.Information, null, Some.MessageTemplate(), new LogEventProperty[0]);
            TestRollingEventSequence(e1, e2);
        }

        void TestRollingEventSequence(params LogEvent[] events)
        {
            var fileName = Some.String() + "{0}.txt";
            var folder = Some.TempFolderPath();
            var pathFormat = Path.Combine(folder, fileName);

            var log = new LoggerConfiguration()
                .WithRollingFileSink(pathFormat)
                .CreateLogger();

            try
            {
                foreach (var @event in events)
                {
                    log.Write(@event);

                    var expected = string.Format(pathFormat, @event.TimeStamp.ToString("yyyy-MM-dd"));
                    Assert.That(System.IO.File.Exists(expected));
                }
            }
            finally
            {
                ((IDisposable)log).Dispose();
                Directory.Delete(folder, true);
            }
        }
    }
}
