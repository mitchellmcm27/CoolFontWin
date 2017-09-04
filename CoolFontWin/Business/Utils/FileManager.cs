using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

using log4net;

namespace PocketStrafe
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
                log.Info("Returning 0");
                return new List<string> { "0" }; // return 0
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
                catch (Exception e) // catch any exception and return 0
                {
                    log.Error("Unable to convert to int: " + e.Message);
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

        /// <summary>
        /// Searches directory for executable file, launching if found.
        /// </summary>
        /// <param name="dir">Path to directory to search within.</param>
        /// <param name="fname">Exectuable file name.</param>
        /// <returns>Returns bool indicating if process started.</returns>
        public static string FindAndLaunch(string dir, string fname)
        {
            string[] drives = Environment.GetLogicalDrives();

            foreach (string dr in drives)
            {
                DriveInfo di = new DriveInfo(dr);
                
                // Here we skip the drive if it is not ready to be read. This
                // is not necessarily the appropriate action in all scenarios.
                if (!di.IsReady)
                {
                    log.Debug("The drive {0} could not be read: " + di.Name);
                    continue;
                }

                log.Info("Searching in drive " + dr + " for directory " + dir + " containing file " + fname);
                string path;
                try
                {
                    path = Path.Combine(dr, dir);
                }
                catch (Exception e)
                {
                    log.Debug("Could not search path: " + e.Message);
                    return string.Empty;
                }

                string exe = FirstOcurrenceOfFile(path, fname);
                if (exe.Length > 0)
                {
                    try
                    {
                        Process.Start(exe);
                        return exe.Replace(fname,"");
                    }
                    catch (Exception e)
                    {
                        log.Debug("Failed to start process " + exe + ": " + e);
                        return string.Empty;
                    }              
                }
            }
            return string.Empty; 
        }

        /// <summary>
        /// Searches given directory and subdirectories for file matching given string.
        /// </summary>
        /// <param name="dir">Path to directory to search within.</param>
        /// <param name="template">File name to match.</param>
        /// <returns>Returns path to file if found, empty string if not.</returns>
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
