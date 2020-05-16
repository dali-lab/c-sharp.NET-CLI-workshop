using System;
using System.IO;
using System.Collections.Generic;

namespace kronos {
    public class Log {
        public static int Run(LogOptions options, string[] args) {
            // read in tracking file
            TimeTrackingFile timeTracking = TimeTrackingFile.toObject(File.ReadAllText(Program.TRACKING_FILE_PATH));

            if (options.Stop || options.Close || options.Done) {
                // if not currently tracking anything, send error
                if (timeTracking.currentTracking == null) {
                    // write out error message
                    ConsoleColor userDefaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No work session in progress");
                    Console.WriteLine("Run kronos log to start a work session");
                    Console.ForegroundColor = userDefaultColor;
                } 
                // if currently tracking, close out and save
                else {
                    timeTracking.currentTracking.endTime = DateTime.Now;
                    timeTracking.currentTracking.totalHours = (timeTracking.currentTracking.endTime - timeTracking.currentTracking.startTime).TotalHours;

                    // figure out message
                    List<string> trimmedArgs = new List<string>(args);
                    trimmedArgs.RemoveAll(s => s.StartsWith("-") || s == "log");

                    if (trimmedArgs.Count > 0) {
                        timeTracking.currentTracking.message = trimmedArgs.ToArray()[0];
                    }

                    // move current tracking to historicals
                    timeTracking.previousTimes.Add(timeTracking.currentTracking);
                    timeTracking.currentTracking = null;

                    // save
                    TimeTrackingFile.SaveToFile(timeTracking);

                    // write out success message
                    ConsoleColor userDefaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully closed work session");
                    Console.ForegroundColor = userDefaultColor;
                }
            } else {
                // if not currently tracking anything, start new tracker
                if (timeTracking.currentTracking == null) {
                    timeTracking.currentTracking = new TimeTrackingFile.TimeTrackingInstance();
                    timeTracking.currentTracking.startTime = DateTime.Now;

                    TimeTrackingFile.SaveToFile(timeTracking);

                    // write out success message
                    ConsoleColor userDefaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully started work session");
                    Console.ForegroundColor = userDefaultColor;
                } 
                // if currently tracking, send error because can't start another
                else {
                    // write out error message
                    ConsoleColor userDefaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Work session already in progress");

                    if (timeTracking.currentTracking.message.Length > 0) {
                        Console.WriteLine("Work session message: " + timeTracking.currentTracking.message + "\n");
                    }

                    Console.WriteLine("Run kronos log --done to finish the current work session");
                    Console.ForegroundColor = userDefaultColor;
                }
            }

            return 0;
        }
    }
}
