using CommandLine;

namespace kronos {

    [Verb("clean", HelpText = "Clean all kronos tracking data")]
    public class CleanOptions {

          [Option('f', "force", Default = false, HelpText = "Don't ask for confirmation")]
          public bool Force { get; set; }
    }
}
