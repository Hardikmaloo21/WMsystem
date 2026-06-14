using Microsoft.AspNetCore.Mvc;
using WMS.Application.Common.Models;
using Asp.Versioning;
namespace WMS.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class ReportsController : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<string>> Get() => Ok(ApiResponse<string>.Success("Reports endpoint ready"));
}
