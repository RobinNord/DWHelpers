
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ConvertStructureDFToSQLColumns
{
    public static class Convert
    {
        private const string TypeString = "String";
        private const string TypeInt32 = "Int32";
        private const string TypeBoolean = "Boolean";
        private const string TypeDecimal = "Decimal";
        private const string TypeDateTime = "DateTime";
        private const string TypeInt64 = "Int64";

        [FunctionName("Convert")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();

            var data = JsonConvert.DeserializeObject<List<Models.Structure>>(requestBody);

            var str = "";

            foreach (var item in data)
            {
                var type = GetType(item);
                if (type == null) throw new System.Exception("COULDN'T FIND TYPE " + item.type);

                str += "\n[" + item.name + "]" + type + ",";
            }


            return (ActionResult)new OkObjectResult(str);
        }


        private static string GetType(Models.Structure model)
        {
            switch (model.type)
            {
                case TypeString:
                    return " nvarchar(1000)";
                case TypeInt32:
                    return " [int] NULL";
                case TypeBoolean:
                    return " bit";
                case TypeDecimal:
                    return " decimal(38,20)";
                case TypeDateTime:
                    return " datetime";
                case TypeInt64:
                    return " [bigint] NULL";
                default:
                    return null;
            }
        }
    }
}
