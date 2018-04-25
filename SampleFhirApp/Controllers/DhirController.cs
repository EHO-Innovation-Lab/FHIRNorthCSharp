using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SampleFhirApp.Models.Builder;
using SampleFhirApp.Models.Message;
using SampleFhirApp.Services;

namespace SampleFhirApp.Controllers
{
    public class DhirController : ServiceController
    {
        public DhirController(DhirService service)
        {
            Service = service;
        }

        private static string BaseUri = "http://lite.innovation-lab.ca:9443/dhir-lite/Immunization?patient.identifier=https://ehealthontario.ca/API/FHIR/NamingSystem/ca-on-panorama-immunization-id|";

        public IActionResult SendMessage(RequestModel model)
        {
            return CreateResponse(model);
        }

        [HttpPost]
        public IActionResult BuildMessage(DhirBuilderModel model)
        {
            //Base Url
            var message = new StringBuilder();
            message.Append(BaseUri);

            //Add Immunization Id
            message.Append(model.ImmunizationId);

            //Check which resources should be included
            if (model.IncludePatient)
                message.Append("&_include=Immunization:patient");

            if (model.IncludeProvider)
                message.Append("&_include=Immunization:performer");

            if (model.IncludeRecommendation)
                message.Append("&_revinclude:recurse=ImmunizationRecommendation:patient");

            //Add Format Type
            message.Append("&_format=" + model.Format);


            var requestModel = new RequestModel
            {
                Format = model.Format,
                RequestMessage = new UriBuilder(message.ToString()).Uri,
                Id = Guid.NewGuid(),
                MessageType = "Dhir"
            };

            //Generate headers
            GenerateRequestHeaders(requestModel, SenderId);


            return PartialView("~/Views/Message/_Request.cshtml", requestModel);
        }

        public override void GenerateRequestHeaders(RequestModel model, string senderId)
        {
            model.Headers = new Dictionary<string, string>
                {
                    { "X-Sender-Id", senderId },
                    { "X-License-Text", "I hereby accept the service agreement here: https://innovation-lab.ca/media/1147/innovation-lab-terms-of-use.pdf" },
                    { "Immunizations_Context", "ew0KInBpbiI6Ijk2Y2FlMzVjZThhOWIwMjQ0MTc4YmYyOGU0OTY2YzJjZTFiODM4NTcyM2E5NmE2YjgzODg1OGNkZDZjYTBhMWUiDQp9" },
                    { "X-ClientTxID", model.Id.ToString() }
                };
        }
    }
}