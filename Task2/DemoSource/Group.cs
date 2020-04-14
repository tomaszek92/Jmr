using System.Collections.Generic;

namespace Task2.DemoSource
{
    public class Group
     {
         public string Id { get; set; }
         public string Label { get; set; }
         public IEnumerable<Person> People { get; set; }
     }
}
