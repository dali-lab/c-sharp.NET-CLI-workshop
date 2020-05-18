using System;
using System.IO;

namespace kronos {
    public class LogHandler {
        public static int Run(LogOptions options, string[] args) {
            // read in tracking file and mark default console color
            TimeTrackingFile timeTracking = TimeTrackingFile.toObject(File.ReadAllText(Program.TRACKING_FILE_PATH));
            ConsoleColor userDefaultColor = Console.ForegroundColor;

            if (timeTracking == null) {
                // write out error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Encountered error generating kronos time.");
                Console.WriteLine("Open " + Program.TRACKING_FILE_PATH + " to view all times.");
                Console.WriteLine("Run kronos clean to fix and reset (note you will lose all previously logged times.");
                Console.ForegroundColor = userDefaultColor;

                return 1;
            }

            DateTime startTime;

            if (options.Start.Length == 0) {
                // write out error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: No start time provided. Use --start to denote a start time.");
                Console.ForegroundColor = userDefaultColor;

                return 2;
            }

            if (!DateTime.TryParse(options.Start, out startTime)) {
                // write out error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Start time not formatted properly.");
                Console.ForegroundColor = userDefaultColor;

                return 3;
            }

            DateTime endTime;

            if (options.Stop.Length == 0) {
                // write out error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: No end time provided. Use --stop to denote an end time.");
                Console.ForegroundColor = userDefaultColor;

                return 2;
            }

            if (!DateTime.TryParse(options.Stop, out endTime)) {
                // write out error message
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: End time not formatted properly.");
                Console.ForegroundColor = userDefaultColor;

                return 3;
            }

            TimeTrackingFile.TimeTrackingInstance instance = new TimeTrackingFile.TimeTrackingInstance();
            instance.startTime = startTime;
            instance.endTime = endTime;
            instance.message = options.Message;
            instance.totalHours = (endTime - startTime).TotalHours;

            // add to array of previous times and save
            timeTracking.previousTimes.Add(instance);
            TimeTrackingFile.SaveToFile(timeTracking);

            // write out success message
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Successfully logged work session.");
            Console.ForegroundColor = userDefaultColor;

            return 0;
        }
    }
}
