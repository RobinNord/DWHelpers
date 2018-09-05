
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
    public static class Function3
    {
        [FunctionName("Insert")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();

            var columnList = requestBody.Split(',');

            var strInsert = "INSERT (";
            var strValues = "VALUES (";

            foreach (var item in columnList)
            {
                string replacement = Regex.Replace(item, @"\t|\n|\r", "").Trim();
                strInsert += replacement + ", ";
                strValues += "s." + replacement + ", "; 
            }

            strInsert = strInsert.Remove(strInsert.Length - 2).Trim();
            strValues = strValues.Remove(strValues.Length - 2).Trim();

            strInsert += ")";
            strValues += ")";

            var str = strInsert + "\n" + strValues + ";";

            return (ActionResult)new OkObjectResult(str);
        }
    }
}
