using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

using log4net;

namespace CoolFont.Business
{
    public static class FileManager
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IEnumerable<string> TryToReadLinesFromFile(string filename)
        {
            log.Info("Reading lines from text file " + filename);
            try
            {
                return File.ReadLines(filename);
            }
            catch (Exception e)
            {
                log.Error("Unable to read port file: " + e.Message);
                log.Info("Returning null");
                return null;
            }
        }

        public static List<int> LinesToInts(IEnumerable<string> lines)
        {
            log.Info("Converting lines to ports.");
            var ints = new List<int>();
            foreach (var line in lines)
            {
                try
                {
                    ints.Add(Convert.ToInt32(line));
                }
                catch (FormatException fe)
                {
                    log.Error("Unable to convert to int: " + fe.Message);
                    log.Info("Setting port to 0");
                    ints.Add(0);
                }
            }    
            return ints;
        }

        public static void WriteLinesToFile(string[] lines, string filename)
        {
            try
            {
                File.AppendAllLines(filename, lines);
                log.Info("Wrote to file: " + String.Join("\n", lines));
            }
            catch (Exception e)
            {
                log.Info("Error: " + e.Message);
            }
        }

        public static void WritePortToLine(int port, int line, string filename)
        {
            string[] linesFromFile;
            if (!File.Exists(filename))
            {
                log.Info("Port file doesn't exist. Creating file: " + filename);
                File.Create(filename).Dispose();

            }
            try
            {
                log.Info("Port file exists, reading all lines.");
                linesFromFile = File.ReadAllLines(filename);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.Error("Did not write any port to file.");
                return;
            }

            // assume 1 port per line
            if (line < linesFromFile.Length)
            {
                // update port found at line
                linesFromFile[line] = port.ToString();
            }
            else
            {
                // append line to list of ports
                List<string> stringList = new List<string>(linesFromFile);
                stringList.Add(port.ToString());
                linesFromFile = stringList.ToArray();
            }

            // write updated list of ports to file
            try
            {
                File.WriteAllLines(filename, linesFromFile);
                log.Info("Wrote port to file: " + port.ToString());
            }
            catch (Exception e)
            {
                log.Error("Could not write port " + port.ToString() + "to line " + line.ToString() + "of file " + filename + ": " + e.Message);
            }
        }

        public static bool FindAndLaunch(string dir, string fname)
        {
            log.Info("Searching in " + dir + " for " + fname);
            string exe = FirstOcurrenceOfFile(dir, fname);
            if (exe.Length > 0)
            {
                Process.Start(exe);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string FirstOcurrenceOfFile(string dir, string template)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    log.Info("Searching in " + d);
                    foreach (string f in Directory.GetFiles(d, template))
                    {
                        log.Info("Found " + f);
                        return f;
                    }
                }
            }
            catch
            {
                return "";
            }
            return "";
        }
    }
}
