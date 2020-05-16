using System;
using System.IO;

namespace kronos {
    public class Clean {
        public static int Run(CleanOptions options) {
            bool proceed = options.Force;

            if (!options.Force) {
                Console.WriteLine("This operation will delete all kronos tracking data.");
                Console.Write("Are you sure you would like to proceed? (Y/N) ");
                string answer = Console.ReadLine();
                Console.Write(answer);
                proceed = (answer.ToUpper() == "Y" || answer.Length == 0);
            }

            if (proceed) {
                File.WriteAllText(Program.TRACKING_FILE_PATH, TimeTrackingFile.GenerateEmptyTrackingJson());

                // write out success message
                ConsoleColor userDefaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully deleted all tracking data");
                Console.ForegroundColor = userDefaultColor;
            }

            return 0;
        }
    }
}
