﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class DisplayTableAttribute : Attribute
    {
        public string TableClass { get; set; }
        public string Header { get; set; }
        public string HeaderClass { get; set; }
        public string ColClass { get; set; }
        public string ValueFormat { get; set; }
    }
}
