using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFhirApp.Models.Message
{
    public class RequestModel
    {
        public Uri RequestMessage { get; set; }

        public Dictionary<string,string> Headers { get; set; }

        public string Format { get; set; }

        public Guid Id { get; set; }

        public string MessageType { get; set; }
    }
}
