using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using Fx_validator_email_phone.Validation;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace Fx_validator_email_phone
{
    public static class LauncherFunction
    {
        [FunctionName("validator")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "validator")] HttpRequest req)
        {
            //Recibe el flujo de entrada
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            //Parametros del query request
            string email = req.Query["email"];
            string cellphone = req.Query["phone_number"];

            //Deserealiza el request
            dynamic dataRequest = JsonConvert.DeserializeObject(requestBody);
            email = email ?? dataRequest?.email;
            cellphone = cellphone ?? dataRequest?.phone_number;

            //Instancia el validaor
            Validator validator = new Validator();

            //Valida que venga alguno de los dos parametros
            if (validator.IsEmpty(email) && validator.IsEmpty(cellphone))
            {
                return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Bad request, email or cellphone are necessary, please check the parameters", Encoding.UTF8, "application/text")
                };

            } 
            else {

             //Valida Correo
                if (!validator.IsEmpty(email)) {
                    if (validator.IsEmail(email))
                    {
                        //Logica para consumir el endpoint de validacion de Email
                    }
                    else {
                        return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("Bad request, email is incorrect", Encoding.UTF8, "application/text")
                        };
                    }
                }

                //Valida Celular
                if (!validator.IsEmpty(cellphone)) {
                    if (validator.IsCellPhoneNumber(cellphone))
                    {
                        string url = "https://metropolis-api-phone.p.rapidapi.com/analysis?country=CO&telephone=" + cellphone + "";
                        var client = new RestClient(url);
                        var request = new RestRequest(Method.GET);
                        request.AddHeader("x-rapidapi-host", "metropolis-api-phone.p.rapidapi.com");
                        request.AddHeader("x-rapidapi-key", "214dca36efmsh95866f13b5b7469p10eab2jsn2aab0b30de8a");
                        IRestResponse response = client.Execute(request);
                        JObject json = JObject.Parse(response.Content);

                        string jsonString = JsonConvert.SerializeObject(json);
                        return new HttpResponseMessage(statusCode: HttpStatusCode.OK)
                        {
                            Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
                        };

                    }
                    else {
                        return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("Bad request, cellphone is incorrect", Encoding.UTF8, "application/text")
                        };
                    }

                }
                }

                return new HttpResponseMessage(statusCode: HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Bad request, email or cellphone are necessary, please check the parameters", Encoding.UTF8, "application/text")
                };

        }
    }
}

