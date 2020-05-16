using CommandLine;

namespace kronos {

    [Verb("log", HelpText = "Log basic kronos time")]
    public class LogOptions {

          [Option("stop", Default = false, HelpText = "Stop logging time (call at end of work session)")]
          public bool Stop { get; set; }

          [Option("close", Default = false, HelpText = "Stop logging time (call at end of work session)")]
          public bool Close { get; set; }

          [Option("done", Default = false, HelpText = "Stop logging time (call at end of work session)")]
          public bool Done { get; set; }
    }
}
