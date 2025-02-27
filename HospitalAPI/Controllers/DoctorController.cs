using System.Net;
using HospitalAPI.Models;
using HospitalAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly HospitalDBService _hospitalDBService;
        public DoctorController(HospitalDBService hospitalDBService)
        {
            _hospitalDBService = hospitalDBService;
        }
        [HttpGet]

        public async Task<IEnumerable<Doctor>> GetDoctors()
        {
            var doctors = await _hospitalDBService.GetDoctors();
            return doctors;
        }
        [HttpGet("{id}")]
        public async Task<IEnumerable<Doctor>> GetDoctorById(int id)
        {
            var doctorbyid = await _hospitalDBService.GetDoctorById(id);
            return doctorbyid;
        }
        [HttpPost]
        public async Task<IActionResult> AddDoctor(Doctor doctor)
        {
            var newdoctor = await _hospitalDBService.AddDoctor(doctor);
            return StatusCode((int)HttpStatusCode.Created, newdoctor);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateDoctor(Doctor doctor)
        {
            var response = await _hospitalDBService.UpdateDoctor(doctor);
            return StatusCode((int)HttpStatusCode.Created, response);
        }
    }
}
