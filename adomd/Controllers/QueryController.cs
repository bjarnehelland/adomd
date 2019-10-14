using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AnalysisServices.AdomdClient;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace adomd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly string connection = "Data Source=devtest2;Catalog=pbLarry_Retail_Foundation";

        [HttpGet]
        public ActionResult Get()
        {
            var query = @"SELECT 
{[Measures].[ItemGrossMarginAmt],[Measures].[ItemSalesNetAmt]} ON COLUMNS,  
{[Item].[ItemGrouping].[All Items].children} ON ROWS 
FROM [Retail]";

            var execute = new ExecuteQuery(connection, query);
            return new OkObjectResult(execute.Run());
        }

        [HttpPost]
        public ActionResult Post([FromBody] string query)
        {
            var execute = new ExecuteQuery(connection, query);
            return new OkObjectResult(execute.Run());
        }

       
    }
}
