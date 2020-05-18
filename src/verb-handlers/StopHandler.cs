using System;
using System.IO;
using System.Collections.Generic;

namespace kronos {
    public class StopHandler {
        public static int Run(StopOptions options, string[] args) {
            // read in tracking file
            TimeTrackingFile timeTracking = TimeTrackingFile.toObject(File.ReadAllText(Program.TRACKING_FILE_PATH));

            if (timeTracking == null) {
                // write out error message
                ConsoleColor userDefaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Encountered error generating kronos time.");
                Console.WriteLine("Open " + Program.TRACKING_FILE_PATH + " to view all times.");
                Console.WriteLine("Run kronos clean to fix and reset (note you will lose all previously logged times.");
                Console.ForegroundColor = userDefaultColor;

                return 1;
            }

            // if not currently tracking anything, send error
            if (timeTracking.currentTracking == null) {
                // write out error message
                ConsoleColor userDefaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: No work session in progress.");
                Console.WriteLine("Run kronos log to start a work session.");
                Console.ForegroundColor = userDefaultColor;

                return 2;
            } 
            // if currently tracking, close out and save
            else {
                timeTracking.currentTracking.endTime = DateTime.Now;
                timeTracking.currentTracking.totalHours = (timeTracking.currentTracking.endTime - timeTracking.currentTracking.startTime).TotalHours;

                // figure out message
                List<string> trimmedArgs = new List<string>(args);
                trimmedArgs.RemoveAll(s => s.StartsWith("-") || s == "stop");

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
                Console.WriteLine("Successfully closed work session.");
                Console.ForegroundColor = userDefaultColor;

                return 0;
            }
        }
    }
}
