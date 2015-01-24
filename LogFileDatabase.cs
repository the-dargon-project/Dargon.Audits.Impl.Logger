using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dargon.Audits.Logger {
   class LogFileDatabase : Database {
      public string Path { get; set; }

      public LogFileDatabase(string path) { Path = path; }

      public void Add(AuditEvent auditEvent) {
         using (StreamWriter writer = File.AppendText(Path)) {
            string text = auditEvent.EventType + "\t" + auditEvent.EventKey + "\t" + auditEvent.EventMessage;
            if (auditEvent.EventData != null)
               text += "\t" + auditEvent.EventData;
            writer.WriteLine(text);
         }
      }

      public IList<AuditEvent> Get() { return Get(0); }

      public IList<AuditEvent> Get(int index) {
         List<AuditEvent> auditEvents = new List<AuditEvent>();
         int i = 0;

         using (StreamReader reader = File.OpenText(Path)) {
            while (!reader.EndOfStream) {
               if (i < index) {
                  reader.ReadLine();
                  i++;
               } else {
                  auditEvents.Add(CreateAuditEventFromString(reader.ReadLine()));
               }
            }
         }
         return auditEvents;
      }

      private AuditEvent CreateAuditEventFromString(string str) {
         string[] parts = str.Split('\t');
         AuditEventType type;
         bool success = Enum.TryParse(parts[0], out type);
         if (!success) {
            throw new Exception();
         } else {
            return new AuditEvent(type, parts[1], parts[2], (parts.Length == 4 ? parts[3] : null));
         }
      }
   }
}
