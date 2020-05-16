using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace kronos {
    public class TimeTrackingFile {
        public TimeTrackingInstance currentTracking;
        public List<TimeTrackingInstance> previousTimes;

        public static void SaveToFile(String json) {
            File.WriteAllText(Program.TRACKING_FILE_PATH, json);
        }

        public static void SaveToFile(TimeTrackingFile obj) {
            SaveToFile(toJSON(obj));
        }

        public static TimeTrackingFile toObject(String json) {
            return JsonConvert.DeserializeObject<TimeTrackingFile>(json);
        }
        
        public static String toJSON(TimeTrackingFile obj) {
            return JsonConvert.SerializeObject(obj);
        }

        public String toJSON() {
            return JsonConvert.SerializeObject(this);
        }

        public static String GenerateEmptyTrackingJson() {
            TimeTrackingFile t = new TimeTrackingFile();
            t.currentTracking = null;
            t.previousTimes = new List<TimeTrackingFile.TimeTrackingInstance>();

            return TimeTrackingFile.toJSON(t);
        }

        public class TimeTrackingInstance {
            public DateTime startTime;
            public DateTime endTime;
            public Double totalHours;
            public String message;
        }
    }   
}
