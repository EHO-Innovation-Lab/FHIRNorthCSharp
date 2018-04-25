using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFhirApp.Models.Message
{/// <summary>
 /// Model to hold the system, code and display of a FHIR coding system
 /// </summary>
    public class Coding
    {
        public string System { get; set; }

        public string Code { get; set; }

        public string Display { get; set; }

    }
}
