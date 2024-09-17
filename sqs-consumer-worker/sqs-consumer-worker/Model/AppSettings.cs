using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqs_consumer_worker.Model
{
    public class AppSettings
    {
        public string QueueUrl { get; set; }
        public int MaxMessages { get; set; }
        public int WaitTimeSeconds { get; set; }
    }
}

