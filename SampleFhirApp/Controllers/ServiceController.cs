using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleFhirApp.Models.Message;
using SampleFhirApp.Services;

namespace SampleFhirApp.Controllers
{
    public class ServiceController : Controller
    {
        public Service Service { get; set; }

        /// <summary>
        /// Sender Id is Generated when account with innovation lab is created. Create an account https://innovation-lab.ca/register/ to recieve your unique sender identifier
        /// </summary>
        protected static string SenderId = "Unique-Sender-Identifier";

        protected IActionResult CreateResponse(RequestModel model)
        {
            var response = new ResponseModel();
            GenerateRequestHeaders(model, SenderId);

            try
            {
                response = Service.Send(model);
            }
            catch (Exception e)
            {
                response.Response = "An error occurred when attempting to send the request\n" + e.Message;
            }
            return PartialView("~/Views/Message/_Response.cshtml", response);

        }

        /// <summary>
        /// Generates request headers for http message
        /// </summary>
        /// <param name="model"></param>
        public virtual void GenerateRequestHeaders(RequestModel model, string senderId)
        {
            model.Headers = new Dictionary<string, string>
                {
                    { "X-Sender-Id", senderId },
                    { "X-License-Text", "I hereby accept the service agreement here: https://innovation-lab.ca/media/1147/innovation-lab-terms-of-use.pdf" },
                    { "X-ClientTxID", model.Id.ToString() }
                };
        }
    }
}