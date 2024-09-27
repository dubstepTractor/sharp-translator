using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator.Core
{
    public class Node
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public List<Node> Children { get; set; } = new List<Node>();
    }
}
