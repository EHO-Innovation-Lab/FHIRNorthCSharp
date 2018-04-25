using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFhirApp.Models.Builder
{
    public class DhirBuilderModel
    {

        [Display(Name = "Ontario Immunization Id")]
        [Required]
        public string ImmunizationId { get; set; }

        public string Format { get; set; }

        [Display(Name = "Patient")]
        public bool IncludePatient { get; set; }

        [Display(Name = "Provider")]
        public bool IncludeProvider { get; set; }

        [Display(Name = "Immunization Recommendation")]
        public bool IncludeRecommendation { get; set; }
        
        
    }
}
