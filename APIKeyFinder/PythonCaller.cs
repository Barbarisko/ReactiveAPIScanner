using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace APIKeyFinder
{
    public class PythonCaller
    {
        ProcessStartInfo psi;
        string script_path;
        public PythonCaller(string script_path = "key_scanner.py")
        {
            this.script_path = script_path;
            psi = new ProcessStartInfo();
            psi.FileName = Environment.GetEnvironmentVariable("PYTHON_PATH");
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
        }

        string Start(string input)
        {
            psi.Arguments = $"\"{script_path}\" \"{input}\"";

            string results;
            string errors;
            using (var process = Process.Start(psi))
            {
                results = process.StandardOutput.ReadToEnd();
                errors = process.StandardError.ReadToEnd();
            }

            if (errors.Length > 0)
            {
                throw new Exception("Python failed with " + errors);
            }

            return results;
        }

        public List<ScanResult> Scan(string input)
        {
            {
                string python_output = Start(input);
                var res = new List<ScanResult>();
                if (python_output.Length <= 0)
                    return res;

                dynamic results = Newtonsoft.Json.Linq.JObject.Parse(python_output);
                foreach (dynamic result in results.data)
                {
                    string line = result.line;
                    int num = result.line_num;
                    res.Add(new ScanResult(num, line));
                }

                return res;
            }
        }
    }
}
