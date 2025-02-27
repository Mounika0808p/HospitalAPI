using System.Net;
using System.Runtime.InteropServices;
using HospitalAPI.Models;
using HospitalAPI.Services;
using HospitalAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    public class UserController : Controller
    {
            private readonly HospitalDBService _hospitalDBService;
            public UserController(HospitalDBService hospitalDBService)
            {
                _hospitalDBService = hospitalDBService;
            }
       
        [HttpPost("Validate")]
        public async Task<IActionResult> ValidateUser(string username)
        {
            var response = await _hospitalDBService.ValidateUser(username);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
        [HttpPost("AddUserdetails")]
        public async Task<IActionResult> AddUserdetails(User user)
        {
            var response = await _hospitalDBService.AddUserdetails(user);
            return StatusCode((int)HttpStatusCode.Created, response);
        }
        
        [HttpPost("SqlValidate")]
        public async Task<IActionResult> SqlValidateUser(string username)
        {
            var response = await _hospitalDBService.SqlValidateUser(username);
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
    }

}
