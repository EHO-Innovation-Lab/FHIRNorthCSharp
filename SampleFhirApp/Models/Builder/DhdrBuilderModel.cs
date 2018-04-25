using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFhirApp.Models.Builder
{
    public class DhdrBuilderModel
    {
        [Display(Name ="Health Card Number")]
        [Required]
        public string HealthCardNumber { get; set; }

        [Display (Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        public string Gender { get; set; }

        public DateTime? Date { get; set; }

        [Display(Name = "Date Modifier")]
        public string DateModifier { get; set; }

        [Display(Name ="Sort Order")]
        public string SortOrder { get; set; }

        public string Format { get; set; }

        public string QueryType { get; set; }
    }
}
