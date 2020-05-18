using CommandLine;

namespace kronos {

    [Verb("log", HelpText = "Log previous work session")]
    public class LogOptions {

          [Option("start", Default = "", HelpText = "Start time of work session")]
          public string Start { get; set; }

          [Option("stop", Default = "", HelpText = "End time of work session")]
          public string Stop { get; set; }

          [Option('m', "message", Default = "", HelpText = "Message for work session")]
          public string Message { get; set; }
    }
}
