using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sqs_consumer_worker.Models
{
    public class SQSMessageModel
    {
        public string MessageId { get; set; }
        public string Body { get; set; }
    }
}

