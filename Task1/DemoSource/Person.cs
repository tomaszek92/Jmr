using System;
using System.Collections.Generic;
using System.Text;

namespace Task1.DemoSource
{
    public class Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<EmailAddress> Emails { get; set; }
    }
}
