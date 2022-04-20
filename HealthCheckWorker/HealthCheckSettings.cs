using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCheckWorker
{
    public class HealthCheckSettings
    {
        public string Url { get; set; }
        public int IntervalMs { get; set; }
    }
}
