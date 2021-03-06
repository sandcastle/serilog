﻿using System.Threading;
using Serilog.Core;
using Serilog.Events;

namespace Harness
{
    class ThreadIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent)
        {
            logEvent.AddPropertyIfAbsent("ThreadId", Thread.CurrentThread.ManagedThreadId);
        }
    }
}