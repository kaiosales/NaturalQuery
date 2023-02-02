using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NaturalQuery.EntityFramework.Test
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestDescriptionAttribute : Attribute
    {
        public string Description { get; private set; }

        public TestDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
