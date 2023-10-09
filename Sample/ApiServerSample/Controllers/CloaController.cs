using Microsoft.AspNetCore.Mvc;
using MyCloa.Common;
using MyCloa.Common.Api;
using MyCloa.Common.Command;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("/api")]
    public class CloaController : Controller
    {
        private IApiHelper _helper;
        public CloaController(IApiHelper helper)
        {
            _helper = helper;
        }
        [Route("report")]
        public string Report(string request)
        {
            return string.Empty;
        }
        [Route("invoke")]
        [HttpPost]
        public async Task<string> Invoke(RequestStringEntity requestStringEntity)
        {
            return await _helper.Call<CommandData>(requestStringEntity);
        }
        [Route("download")]
        public ResultEntity Download(RequestStringEntity requestStringEntity)
        {
            return new ResultEntity();
        }
    }
}
