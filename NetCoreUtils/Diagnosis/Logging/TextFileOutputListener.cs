using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NetCoreUtils.Diagnosis.Logging
{
    public class TextFileOutputListener : IOutput
    {
        TextWriter writer = null;

        public TextFileOutputListener(string filePath, string fileName)
        {
            if(!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            string fullFilePath = Path.Combine(filePath, fileName);

            writer = File.AppendText(fullFilePath);
        }

        public void Dispose()
        {
            if(writer != null)
                writer.Dispose();
        }

        public void Write(string message)
        {
            writer.Write(message);
        }

        public void WriteLine(string message)
        {
            writer.WriteLine(message);
        }
    }
}
