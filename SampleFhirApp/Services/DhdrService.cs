using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleFhirApp.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFhirApp.Services
{
    public class DhdrService : Service
    {
        public DhdrService(string endpoint, FhirJsonSerializer jsonSerializer, FhirXmlSerializer xmlSerializer)
            : base(endpoint, jsonSerializer, xmlSerializer)
        {
        }

        public override ResponseModel Send(RequestModel model)
        {
            Headers = model.Headers;
            FhirClient.OnBeforeRequest += FhirClient_OnBeforeRequest;
            var response = new ResponseModel { PatientNames = new List<Name>(), Medications = new List<Models.Message.Coding>(), PractitionerNames = new List<Name>() };
            try
            {
                //Send the message to the server using the FhirClient. Sends a URI with the appropriate headers and returns
                //a fhir object containing the resulting resources
                var result = FhirClient.Get(model.RequestMessage.AbsoluteUri);

                var bundle = result as Bundle;
                ParseBundle(bundle, response);

                response.Response = FormatResult(model.Format, result);
            }
            catch (Exception e)
            {
                response.Response = "An unexpected error occurred: " + e.Message;
            }

            return response;
        }

        protected override void ParseBundle(Bundle bundle, ResponseModel response)
        {
            if (bundle == null)
                return;
            //Retrieve information from bundle
            foreach (var entry in bundle.Entry)
            {
                //Check if the entry is of the type we are wanting to work with and contains the appropriate resources
                if (!(entry.Resource is MedicationDispense dispense))
                    continue;

                if (!(dispense.Contained != null && dispense.Contained.Any()))
                    continue;

                //Iterate over each contained resource. Can be any resource type referenced within the containing resource
                foreach (var resource in dispense.Contained)
                {
                    //Based on the resource type. Retrieve information
                    if (resource is Patient patient)
                    {
                        response.PatientNames.AddRange(ParsePatient(patient));

                    }
                    else if (resource is Medication medication)
                    {
                        ParseMedication(medication, response);

                    }
                    else if (resource is Practitioner practitioner)
                    {
                        response.PractitionerNames.Add(ParsePractitioner(practitioner));
                    }
                }
            }
        }
    }
}
