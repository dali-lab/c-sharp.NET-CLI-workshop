using System;
using System.IO;
using System.Collections.Generic;

namespace kronos {
    public class Track {
        public static int Run(TrackOptions options, string[] args) {
           // Create a new FileSystemWatcher and set its properties.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                String path = "";

                // figure out message
                List<string> trimmedArgs = new List<string>(args);
                trimmedArgs.RemoveAll(s => s.StartsWith("-") || s == "track");

                if (trimmedArgs.Count > 0) {
                    path = trimmedArgs.ToArray()[0];
                } else {
                    // write out error message
                    ConsoleColor userDefaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: No file or directory path provided");
                    Console.ForegroundColor = userDefaultColor;

                    return 1;
                }

                if (!(File.Exists(path) || Directory.Exists(path))) {
                    // write out error message
                    ConsoleColor userDefaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Provided directory/file path does not exist");
                    Console.ForegroundColor = userDefaultColor;

                    return 2;
                }

                watcher.Path = path;
                watcher.NotifyFilter = NotifyFilters.LastWrite;
                watcher.Filter = "*.*";
                watcher.Changed += new FileSystemEventHandler(OnChanged);

                // begin watching
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to stop tracking " + watcher.Path);
                while (Console.Read() != 'q') ;
            }

            return 0;
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e) {
            FileSystemWatcher watcher = source as FileSystemWatcher;

            // read in tracking file
            TimeTrackingFile timeTracking = TimeTrackingFile.toObject(File.ReadAllText(Program.TRACKING_FILE_PATH));

            if (timeTracking.currentTracking == null) {
                timeTracking.currentTracking = new TimeTrackingFile.TimeTrackingInstance();
                timeTracking.currentTracking.startTime = DateTime.Now;
                timeTracking.currentTracking.message = "Auto-generated based on file changes to " + watcher.Path;

                TimeTrackingFile.SaveToFile(timeTracking);
            } else if ((DateTime.Now - timeTracking.currentTracking.startTime).TotalMinutes >= 15) {
                timeTracking.currentTracking.endTime = DateTime.Now;
                timeTracking.currentTracking.totalHours = (timeTracking.currentTracking.endTime - timeTracking.currentTracking.startTime).TotalHours;
                timeTracking.currentTracking.message = "Auto-generated based on file changes to " + watcher.Path;

                 // move current tracking to historicals
                timeTracking.previousTimes.Add(timeTracking.currentTracking);
                timeTracking.currentTracking = null;

                // save
                TimeTrackingFile.SaveToFile(timeTracking);
            }

            // Specify what is done when a file is changed, created, or deleted.
            // Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        }
    }
}
