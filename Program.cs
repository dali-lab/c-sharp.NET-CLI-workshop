using System;
using System.IO;
using CommandLine;

namespace kronos {
    class Program {
        public static string DIRECTORY_PATH = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), ".kronos");
        public static string TRACKING_FILE_PATH = Path.Combine(DIRECTORY_PATH, "kronos.json");

        static void Main(string[] args) {
            kronosSetup();

            CommandLine.Parser.Default.ParseArguments<LogOptions, ReportOptions, CleanOptions, TrackOptions>(args)
                .MapResult(
                (LogOptions opts) => Log.Run(opts, args),
                (ReportOptions opts) => Report.Run(opts),
                (CleanOptions opts) => Clean.Run(opts),
                (TrackOptions opts) => Track.Run(opts, args),
                errs => 1);
        }

        private static void kronosSetup() {
            // generate home .kronos directory if doesn't exist
            if (!Directory.Exists(DIRECTORY_PATH)) {
                Directory.CreateDirectory(DIRECTORY_PATH);
            }

            // generate tracking file if doesn't exist
            if (!File.Exists(TRACKING_FILE_PATH)) {
                File.WriteAllText(TRACKING_FILE_PATH, TimeTrackingFile.GenerateEmptyTrackingJson());
            }
        }
    }
}
