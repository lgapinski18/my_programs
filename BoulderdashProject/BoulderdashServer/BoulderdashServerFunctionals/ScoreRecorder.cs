using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoulderdashServerFunctionals
{
    public class ScoreRecorder
    {
        private string fileName;

        public ScoreRecorder(string fileName)
        {
            this.fileName = fileName;
        }

        public string writeScore(string msg)
        {
            DateTime now = DateTime.Now;

            string finalMsg = now.ToString() + " " + msg;

            while (finalMsg.EndsWith("\n"))
            {
                finalMsg = finalMsg.Substring(0, finalMsg.Length - 1);
            }

            FileStreamOptions fso = new FileStreamOptions();
            fso.Mode = FileMode.Append;
            fso.Access = FileAccess.Write;

            using (StreamWriter writer = new StreamWriter(fileName, fso))
            {
                writer.Write(finalMsg + "\n");
            }

            return finalMsg;
        }

        public List<string> readScores()
        {
            List<string> lines = new();

            FileStreamOptions fso = new FileStreamOptions();
            fso.Mode = FileMode.OpenOrCreate;
            fso.Access = FileAccess.Read;
            using (StreamReader reader = new StreamReader(fileName, fso))
            {
                string? line = reader.ReadLine();

                while (line != null)
                {
                    Trace.WriteLine(line);
                    lines.Add(line);

                    line = reader.ReadLine();
                }
            }

            return lines;
        }
    }
}
