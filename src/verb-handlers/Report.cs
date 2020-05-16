using System;
using System.IO;

namespace kronos {
    public class Report {
        public static int Run(ReportOptions options) {
            // read in tracking file
            TimeTrackingFile timeTracking = TimeTrackingFile.toObject(File.ReadAllText(Program.TRACKING_FILE_PATH));

            // generate header
            Console.Write("\nKronos Report -- ");
            Console.WriteLine(options.All ? "all time" : "last 14 days");

            // write out current session if in progress
            if (timeTracking.currentTracking != null) {
                Console.WriteLine();
                Console.WriteLine("In progress work session:");
                Console.WriteLine("    Start time: " + 
                    timeTracking.currentTracking.startTime.ToLongDateString() + ", " + 
                    timeTracking.currentTracking.startTime.ToLongTimeString() + 
                    (timeTracking.currentTracking.message.Length > 0 ? ", \n    " + timeTracking.currentTracking.message : ""));
            }

            Console.WriteLine("\nPast work sessions:");
            DateTime currentDate = default(DateTime);

            // write out past work sessions
            foreach (TimeTrackingFile.TimeTrackingInstance t in timeTracking.previousTimes) {
                if (options.All || (DateTime.Now - t.endTime).TotalDays <= 14) {
                    if (currentDate == default(DateTime) || t.startTime.ToShortDateString() != currentDate.ToShortDateString()) {
                        Console.WriteLine("\n" + t.startTime.ToShortDateString());
                    }

                    Console.WriteLine("   " +
                        " Start time: " + t.startTime.ToShortTimeString() + "," +
                        " End time: " + t.endTime.ToShortTimeString() + "," +
                        " Hours: " + Math.Round(t.totalHours, 2) + 
                        (t.message != null && t.message.Length > 0 ? ", Message: " + t.message : ""));

                    currentDate = t.startTime;
                }
            }

            Console.WriteLine();
            return 0;
        }
    }
}
