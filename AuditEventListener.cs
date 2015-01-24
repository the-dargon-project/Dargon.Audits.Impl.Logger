using System;
using System.IO;

namespace Dargon.Audits.Logger {
   public class AuditEventListener {
      private LogFileDatabase database;

      public AuditEventListener(AuditEventBus auditEventBus, string path) {
         database = new LogFileDatabase(path);
         auditEventBus.EventPosted += (sender, auditEvent) => Log(auditEvent);
      }

      private void Log(AuditEvent auditEvent) {
         database.Add(auditEvent);
      }
   }
   
}
