using Common.Context.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Context
{
    [DbModel]
    public class ScheduledJob: BasicModel<ScheduledJob>
    {
        public BasicModel Owner { get; set; }

        public DateTime ExecutionTime { get; set; }
    }
}
