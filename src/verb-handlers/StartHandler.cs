using System;
using System.IO;

namespace kronos {
    public class StartHandler {
        public static int Run(StartOptions options, string[] args) {
            // read in tracking file
            TimeTrackingFile timeTracking = TimeTrackingFile.toObject(File.ReadAllText(Program.TRACKING_FILE_PATH));

            if (timeTracking == null) {
                // write out error message
                ConsoleColor userDefaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Encountered error generating kronos time.");
                Console.WriteLine("Open " + Program.TRACKING_FILE_PATH + " to view all times.");
                Console.WriteLine("Run kronos clean to fix and reset (note you will lose all previously logged times).");
                Console.ForegroundColor = userDefaultColor;

                return 1;
            }

            // if not currently tracking anything, start new tracker
            if (timeTracking.currentTracking == null) {
                timeTracking.currentTracking = new TimeTrackingFile.TimeTrackingInstance();
                timeTracking.currentTracking.startTime = DateTime.Now;

                TimeTrackingFile.SaveToFile(timeTracking);

                // write out success message
                ConsoleColor userDefaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully started work session.");
                Console.ForegroundColor = userDefaultColor;

                return 0;
            } 
            // if currently tracking, send error because can't start another
            else {
                // write out error message
                ConsoleColor userDefaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Work session already in progress.");

                if (timeTracking.currentTracking.message != null && timeTracking.currentTracking.message.Length > 0) {
                    Console.WriteLine("Work session message: " + timeTracking.currentTracking.message + "\n");
                }

                Console.WriteLine("Run kronos stop to finish the current work session.");
                Console.ForegroundColor = userDefaultColor;

                return 2;
            }
        }
    }
}
