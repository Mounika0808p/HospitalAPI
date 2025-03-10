﻿using HospitalAPI.Models;

namespace HospitalAPI.Services.Interfaces
{
    public interface IHospitalDBService
    {
        // Define the method signatures for GetPatients and GetDoctors 
        Task<IEnumerable<Patient>> GetPatients();
        Task<IEnumerable<Doctor>> GetDoctors();

        // Define the method signature for GetPatientById\

        Task<List<Patient>> GetPatientById(int id);


        // Define the method signature for GetDoctorById

        Task<List<Doctor>> GetDoctorById(int id);

        // Define the method signature for AddPatient

        Task<Patient> AddPatient(Patient patient);

        // Define the method signature for AddDoctor
        Task<Doctor> AddDoctor(Doctor doctor);

        // Define the method signature for UpdatePatient 
        Task<Response> UpdatePatient(Patient patient);

        // Define the method signature for updatepatient
        Task<Response> UpdateDoctor(Doctor doctor);

        // ValidateUser from json file
        Task<User> ValidateUser(string username);

        // Define method signature for AddUserdetails
        Task<User> AddUserdetails(User user);

        // ValidateUser from sql server
        Task<User> SqlValidateUser(string username);

    }
}
