using PODWebAPI.Filters;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PODWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    [CustomAuthorize]
    public class CustomBaseController : ApiController
    {
        

    }

}
