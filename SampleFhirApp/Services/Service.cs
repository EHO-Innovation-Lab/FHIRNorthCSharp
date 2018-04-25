using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleFhirApp.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace SampleFhirApp.Services
{
    public abstract class Service
    {
        public string Endpoint { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public FhirClient FhirClient { get; set; }

        public FhirJsonSerializer JsonSerializer { get; set; }

        public FhirXmlSerializer XmlSerializer { get; set; }

        public Service(string endpoint, FhirJsonSerializer jsonSerializer, FhirXmlSerializer xmlSerializer)
        {
            Endpoint = endpoint;
            FhirClient = new FhirClient(Endpoint);
            JsonSerializer = jsonSerializer;
            XmlSerializer = xmlSerializer;
        }

        /// <summary>
        /// Access the fhir endpoint, sending the requested message. Retrieves the response, extracts information from the fhir bundle, and format's the string response appropriately
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public abstract ResponseModel Send(RequestModel model);

        /// <summary>
        /// Parses out contents of a bundle, will perform different operations based on the contained resources
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        protected abstract void ParseBundle(Bundle bundle, ResponseModel response);


        /// <summary>
        /// Add headers to http message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FhirClient_OnBeforeRequest(object sender, BeforeRequestEventArgs e)
        {
            foreach (var header in Headers)
            {
                e.RawRequest.Headers.Add(header.Key, header.Value);
            }
        }

        /// <summary>
        /// Takes a patient object, returning a list of all names contained within the patient
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        public List<Name> ParsePatient(Patient patient)
        {
            if (!(patient.Name != null && patient.Name.Any()))
                return new List<Name>();
            return patient.Name.Select(n => GetName(n)).ToList();

        }

        /// <summary>
        /// Retrieves human name information in string format
        /// </summary>
        /// <param name="humanName"></param>
        /// <returns></returns>
        public Name GetName(HumanName humanName)
        {
            var name = new Name { Last = "", Given = ""};
            if (humanName.Given != null && humanName.Given.Any())
            {
                foreach (var given in humanName.Given)
                {
                    name.Given += given + " ";
                }
            }
            name.Last = humanName.Family.FirstOrDefault();

            return name;
        }

        /// <summary>
        /// Method to extract practitioner name from a practitioner object
        /// </summary>
        /// <param name="practitioner"></param>
        /// <returns></returns>
        public Name ParsePractitioner(Practitioner practitioner)
        {
            if (practitioner.Name == null)
                return null;

            return GetName(practitioner.Name);
        }

        /// <summary>
        /// Method to retrieve Code and display information from a medication object
        /// </summary>
        /// <param name="medication"></param>
        /// <param name="response"></param>
        public void ParseMedication(Medication medication, ResponseModel response)
        {
            if (!(medication.Code != null && medication.Code.Coding != null && medication.Code.Coding.Any()))
                return;

            foreach (var code in medication.Code.Coding)
            {
                response.Medications.Add(new Models.Message.Coding { Code = code.Code ?? "n/a", Display = code.Display ?? "n/a", System = code.System ?? "n/a" });
            }
        }

        /// <summary>
        /// Method to retrieve immunization codes and display information from an immunization
        /// </summary>
        /// <param name="immunization"></param>
        /// <param name="response"></param>
        public void ParseImmunization(Immunization immunization, ResponseModel response)
        {
            if (!(immunization.VaccineCode.Coding != null && immunization.VaccineCode.Coding.Any()))
                return;

            foreach (var code in immunization.VaccineCode.Coding)
            {
                response.Immunizations.Add(new Models.Message.Coding { Code = code.Code ?? "", Display = code.Display ?? "", System = code.System ?? "" });
            }

        }

        /// <summary>
        /// Parses FHIR resource into a string format in either xml or json, based on user preference
        /// </summary>
        /// <param name="format"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string FormatResult(string format, Resource result)
        {
            if (format.Contains("json"))
            {
                var json = JToken.Parse(JsonSerializer.SerializeToString(result));
                return json.ToString(Formatting.Indented);
            }
            else
            {
                var xml = XmlSerializer.SerializeToString(result);
                return System.Xml.Linq.XDocument.Parse(xml).ToString();
            }
        }
    }
}
