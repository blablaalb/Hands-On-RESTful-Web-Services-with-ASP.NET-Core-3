﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Configurations
{
    public class EventBusSettings
    {
        public string HostName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string EventQueue { get; set; }
    }
}
