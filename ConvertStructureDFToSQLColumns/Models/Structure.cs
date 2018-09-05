using System;
using System.Collections.Generic;
using System.Text;

namespace ConvertStructureDFToSQLColumns.Models
{
    public class DFMapping
    {
        public List<Structure> Structures { get; set; }
    }

    public class Structure
    {
        public string name { get; set; }
        public string type { get; set; }
    }
}
