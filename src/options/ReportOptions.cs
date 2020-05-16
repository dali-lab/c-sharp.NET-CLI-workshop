using CommandLine;

namespace kronos {

    [Verb("report", HelpText = "Generate kronos report for last 14 days")]
    public class ReportOptions {
        [Option("all", Default = false, HelpText = "View all kronos tracking data")]
        public bool All { get; set; }
    }
}
