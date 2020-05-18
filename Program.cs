using System;
using System.IO;
using CommandLine;

namespace kronos {
    class Program {
        public static string DIRECTORY_PATH = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), ".kronos");
        public static string TRACKING_FILE_PATH = Path.Combine(DIRECTORY_PATH, "kronos.json");

        static int Main(string[] args) {
            kronosSetup();

            return CommandLine.Parser.Default.ParseArguments<StartOptions, StopOptions, LogOptions, ReportOptions, CleanOptions, TrackOptions>(args)
                .MapResult(
                    (StartOptions opts) => StartHandler.Run(opts, args),
                    (StopOptions opts) => StopHandler.Run(opts, args),
                    (LogOptions opts) => LogHandler.Run(opts, args),
                    (ReportOptions opts) => ReportHandler.Run(opts),
                    (CleanOptions opts) => CleanHandler.Run(opts),
                    (TrackOptions opts) => TrackHandler.Run(opts, args),
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
