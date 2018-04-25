using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFhirApp.Models.Message
{
    public class ResponseModel
    {
        /// <summary>
        /// List of names assigned to a patient
        /// </summary>
        public List<Name> PatientNames { get; set; }

        /// <summary>
        /// List of names assigned to a practitioner
        /// </summary>
        public List<Name> PractitionerNames { get; set; }

        /// <summary>
        /// Immunization codes
        /// </summary>
        public List<Coding> Immunizations { get; set; }

        /// <summary>
        /// Medication codes
        /// </summary>
        public List<Coding> Medications { get; set; }

        public string Response { get; set; }
    }
    
}
