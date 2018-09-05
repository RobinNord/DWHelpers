
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ConvertStructureDFToSQLColumns
{
    public static class Function2
    {
        [FunctionName("Update")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string requestBody = new StreamReader(req.Body).ReadToEnd();

            var columnList = requestBody.Split(',');

            var str = "";

            foreach (var item in columnList)
            {
                string replacement = Regex.Replace(item, @"\t|\n|\r", "").Trim();
                str += "\nt." + replacement + " = s." + replacement + ",";
            }

            return (ActionResult)new OkObjectResult(str);
        }
    }
}
