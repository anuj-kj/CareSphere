using CareSphere.Domains.Core;
using CareSphere.Services.Organizations.Impl;
using CareSphere.Services.Organizations.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CareSphere.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private IOrganizationService OrganizationService { get; set; }

        public OrganizationController(IOrganizationService organizationService)
        {
            OrganizationService = organizationService;
        }
        //[Authorize]
        [HttpGet("all")]
        /// <summary>
        ///  Returns all the organizations
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns>Organization List</returns>
        public async Task<ActionResult<List<Organization>>> GetAllOrganizations()
        {
            try
            {
                return Ok(await OrganizationService.GetOrganizations());
            }
            catch (Exception ex)
            {
                //Log the error i.e., ex.Message
                return NotFound($"Organizations not found {ex.Message}");
            }
        }
    }
}
