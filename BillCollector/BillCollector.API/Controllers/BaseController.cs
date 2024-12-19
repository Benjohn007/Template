using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace BillCollector.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private StringValues appUserHeader;

        protected long ApiUserId
        {
            get
            {
                Request.Headers.TryGetValue("ApiUserId", out appUserHeader);

                if (appUserHeader != StringValues.Empty)
                {
                }
                return default;
            }

        }
    }
}
