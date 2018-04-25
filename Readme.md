# FHIR North 2018 C# Application

Simple implementation of the Hl7 FHIR library in an ASP.NET Core MVC application. Project makes use of the FHIR library to interact with the Digital Health Immunization Repository(DHIR) and Digital Health Drug Repository(DHDR) lite services made available through eHealth Ontario. 

## Getting Started

###Before You Begin

As this project was created in .NET Core, Visual Studio 2017 is required to run this project.

###Downloading the Application

The source code for this application can be found on the Innovation Lab code sharing repository at https://innovation-lab.ca/repository/ViewRepository?id=26902277-1107-4c7d-93b1-a5aa49c1b07d

### Running the Application

To send messages using this application, you will need to create an account on the innovation lab https://innovation-lab.ca/register/ to generate your unique sender identifier. 

This identifier is required to send messages to the DHDR and DHIR endpoints. Once you have generated your sender identifier it will need to be put as the value for SenderId in the Service Controller.


```
/// <summary>
/// Sender Id is Generated when account with innovation lab is created. Create an account https://innovation-lab.ca/register/ to receive your unique sender identifier
/// </summary>
protected static string SenderId = "Unique-Sender-Identifier";
```

###Trouble Shooting the Application

A forum for any questions or concerns with this starter project is being hosted on the innovation lab at https://innovation-lab.ca/discussions/topic/q-a-for-fhir-in-c/. 

## Libraries Used

This project makes use of the Hl7.FHIR STU3 library. https://github.com/ewoutkramer/FHIR-net-api.

This library is used to communicate with the FHIR endpoints, as well as retrieve information out of the bundles. This is the official .NET API for Hl7 FHIR.

## Things to Note

Some potential issues encountered when working with the FHIR library include navigating FHIR's hierarchy in order to access individual elements. In FHIR, types many attributes are not required, and are able to be a variety of datatypes depending on the situation. 

When traversing a FHIR object it is important to be checking the datatype of the element you are working with and checking whether or not that element has been set. If this is not done, it is common for FHIR to encounter type mismatch and null reference exceptions. By type checking and checking that elements have been set, we can avoid many of these errors.

Below is an example of code used to extract code information from an immunization resource. In this example we can see that before extracting the information from the resource, we are ensuring that the element we are examining exists.

```
if (!(immunization.VaccineCode.Coding != null && immunization.VaccineCode.Coding.Any()))
                return;
foreach (var code in immunization.VaccineCode.Coding)
{
    response.Immunizations.Add(new Models.Message.Coding { Code = code.Code ?? "", Display = code.Display ?? "", System = code.System ?? "" });
}
```

## Authors

**Christopher Dekker** - Ideaworks MEDIC
