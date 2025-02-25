using System.Net;
using HospitalAPI.Models;
using HospitalAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly HospitalDBService _hospitalDBService;
        public PatientController(HospitalDBService hospitalDBService)
        {
            _hospitalDBService = hospitalDBService;
        }
        [HttpGet]
        public async Task<IEnumerable<Patient>> GetPatients()
        {
            var patients = await _hospitalDBService.GetPatients();
            return patients;
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<Patient>> GetPatientById(int id)
        {
            var patientbyid = await _hospitalDBService.GetPatientById(id);
            return patientbyid;
        }

        [HttpPost]

        public async Task<IActionResult> AddPatient(Patient patient)
        {
            var newpatient = await _hospitalDBService.AddPatient(patient);
            return StatusCode((int)HttpStatusCode.Created, newpatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, Patient patient)
        {
            await _hospitalDBService.UpdatePatient(id, patient);
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
