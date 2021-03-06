﻿using Hl7.Fhir.Model;
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
    public class DhirService : Service
    {
        public DhirService(string endpoint, FhirJsonSerializer jsonSerializer, FhirXmlSerializer xmlSerializer)
            : base(endpoint, jsonSerializer, xmlSerializer)
        {
        }

        public override ResponseModel Send(RequestModel model)
        {
            var response = new ResponseModel { PatientNames = new List<Name>(), Immunizations = new List<Models.Message.Coding>() };
            Headers = model.Headers;
            FhirClient.OnBeforeRequest += FhirClient_OnBeforeRequest;
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
                if (entry.Resource is Patient patient)
                {
                    response.PatientNames.AddRange(ParsePatient(patient));
                }
                else if (entry.Resource is Immunization immunization)
                {
                    ParseImmunization(immunization, response);
                }
            }
        }

    }
}
