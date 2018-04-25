using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc;
using SampleFhirApp.Models;
using SampleFhirApp.Models.Builder;
using SampleFhirApp.Models.Message;
using SampleFhirApp.Services;

namespace SampleFhirApp.Controllers
{
    public class DhdrController : ServiceController
    {
        public DhdrController(DhdrService service)
        {
            Service = service;
        }
        private static string BaseUri = "http://lite.innovation-lab.ca:9443/dispense-service/MedicationDispense?";

        public IActionResult SendMessage(RequestModel model)
        {
            return CreateResponse(model);
        }

        [HttpPost]
        public IActionResult BuildMessage(DhdrBuilderModel model)
        {
            //Base Url
            var message = new StringBuilder();
            message.Append(BaseUri);

            message.Append("patient:patient.identifier=https://ehealthontario.ca/API/FHIR/NamingSystem/ca-on-patient-hcn|" + model.HealthCardNumber.Trim());

            if (model.QueryType.Equals("3-Point"))
            {
                if(model.DateOfBirth != null)
                {
                    message.Append("&patient:patient.birthdate=" + model.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                }
                message.Append("&patient:patient.gender=" + model.Gender);
            }

            if (model.Date != null)
                message.Append("&whenHandedOver=" + model.DateModifier + model.Date.Value.ToString("yyyy-MM-dd"));
            //Add Format
            message.Append("&_format=" + model.Format);
            var requestModel = new RequestModel
            {
                Format = model.Format,
                RequestMessage = new UriBuilder(message.ToString()).Uri,
                Id = Guid.NewGuid(),
                MessageType = "Dhdr"
            };

            GenerateRequestHeaders(requestModel, SenderId);

            return PartialView("~/Views/Message/_Request.cshtml", requestModel);
        }
    }
}
