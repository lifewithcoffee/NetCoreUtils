using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NetCoreUtils.Diagnosis.Logging
{
    public class TextFileOutputListener : IOutput
    {

        public TextFileOutputListener(string filePath)
        {
        }

        public void Dispose() { }

        public void Write(string message)
        {
            //using (StreamWriter w = File.AppendText("log.txt"))
            //{

            //}
        }

        public void WriteLine(string message)
        {

        }
    }
}
