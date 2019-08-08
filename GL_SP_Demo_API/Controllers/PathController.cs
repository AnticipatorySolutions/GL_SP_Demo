using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GL_SP_Demo_BLL.Route;


namespace GL_SP_Demo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PathController : ControllerBase
    {
        private readonly IGL_Route_BLL _SP_Route_BLL;

        public PathController(IGL_Route_BLL sP_Route_BLL)
        {
            _SP_Route_BLL = sP_Route_BLL;
        }

        // Finds the Route Using A Graph Search
        // GET: api/Path/SP?src=SOURCE_LOCATION_CODE3_ID&dst=DESTINATION_LOCATION_CODE3_ID
        // Param src = source location code 3 value of type string
        // Param dst = destination location code 3 value of type string
        // Returns piped string "|YYZ|DEN|" or message details.

        // EXAMPLE: of 2 Step Trek: api/Path/SP?src=YYZ&dst=LAX
        // EXAMPLE: of 12 Step Trek: api/Path/SP?src=ASF&dst=MAJ
        // FUN EXAMPLES: src= PWM or CRW or SPI or TMS or EIS or ENY or SXB to anywhere
        // FUN EXAMPLES: dst= NKC or GDN or TRD or PGA from anywhere
        // INTERESTING NOTE: LBL|DDC or DDC|LBL otherwise these nodes can be src but not dst

        [HttpGet("SP")]
        public ActionResult<string> Get(
            [FromQuery] string src,
            [FromQuery] string dst)
        {
            try
            {
                string nullCheck = NullCheck(src, dst);
                if (nullCheck != null) { return nullCheck; }
                return _SP_Route_BLL.GetShortestPath(src, dst);
            }
            catch (Exception error)
            {
                return $"{error.Source} - {error.Message} - {error.StackTrace}";
            }
        }

        private string NullCheck(string src, string dst)
        {
            if (src == null && dst == null) { return "Please include src and dst Code 3 values in your query."; };
            if (src == null) { return "Please include src Code 3 value in your query."; };
            if (dst == null) { return "Please include a dst Code 3 value in your query."; };
            return null;
        }

    }
}