using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIKeyFinder
{
    public class ScanResult : Object
    {
        private int line_num { get; }
        private string line_text { get; }

        public ScanResult()
        {
            line_num = -1;
        }
        public ScanResult(int num, string line_text)
        {
            this.line_num = num;
            this.line_text = line_text;
        }

        public override string ToString()
        {
            return "Key at line " + line_num.ToString() + ":\n" + line_text + "\n";
        }
    }
}
