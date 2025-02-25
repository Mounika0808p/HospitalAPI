using System.Data.SqlClient;
using System.Net;
using HospitalAPI.Models;
using HospitalAPI.Services.Interfaces;

namespace HospitalAPI.Services
{
    public class HospitalDBService : IHospitalDBService
    {
        private readonly string _connection;


        public HospitalDBService(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("HospitalConnection");
        }

        public async Task<IEnumerable<Patient>> GetPatients()
        {
            var patients = new List<Patient>();
            /// Connection open
            using (var connection = new SqlConnection(_connection))
            /// Sql command 
            using (var command = new SqlCommand("Select * from Patients", connection))
            {
                connection.Open();
                /// Data reader
                using (var reader = await command.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {
                        patients.Add(new Patient
                        {
                            PatientId = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            DateOfBirth = reader.GetDateTime(2),
                            Gender = reader.GetString(3),
                            ContactNumber = reader.GetString(4)
                        });

                    }

                }
                return patients;
            }
        }
        public async Task<List<Patient>> GetPatientById(int id)
        {
            var patientbyid = new List<Patient>();

            using (var connection = new SqlConnection(_connection))
            using (var command = new SqlCommand("Select * from Patients where PatientId = @PatientId", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@PatientId", id);
                using (var reader = command.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        patientbyid.Add(new Patient
                        {
                            PatientId = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            DateOfBirth = reader.GetDateTime(2),
                            Gender = reader.GetString(3),
                            ContactNumber = reader.GetString(4)
                        });

                    }
                }
                return patientbyid;
            }
        }
        public async Task<IEnumerable<Doctor>> GetDoctors()
        {
            var doctors = new List<Doctor>();

            using (var connection = new SqlConnection(_connection))

            using (var command = new SqlCommand("Select * from Doctors", connection))
            {
                connection.Open();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        doctors.Add(new Doctor
                        {
                            DoctorId = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Specialty = reader.GetString(2),
                            ContactNumber = reader.GetString(3)
                        });
                    }
                }
                return doctors;
            }
        }

        public async Task<List<Doctor>> GetDoctorById(int id)
        {
            var doctorbyid = new List<Doctor>();

            using (var connection = new SqlConnection(_connection))
            using (var command = new SqlCommand("Select * from Doctors where DoctorId = @DoctorId", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@DoctorId", id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctorbyid.Add(new Doctor
                        {
                            DoctorId = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Specialty = reader.GetString(2),
                            ContactNumber = reader.GetString(3)
                        });
                    }
                }
                return doctorbyid;
            }
        }

        public Task<Patient> AddPatient(Patient patient)
        {
            // connection open
            // sql command 
            // execute non query
            // return task
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("insert into Patients values(@FullName, @DateOfBirth, @Gender, @ContactNumber)", connection);
                cmd.Parameters.AddWithValue("@Fullname", patient.FullName);
                cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                cmd.Parameters.AddWithValue("@ContactNumber", patient.ContactNumber);

                int i = cmd.ExecuteNonQuery();
                return Task.FromResult(patient);

            }
        }

        public Task<Doctor> AddDoctor(Doctor doctor)
        {
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("insert into Doctors values(@FullName, @Specialty, @ContactNumber)", connection);
                cmd.Parameters.AddWithValue("@Fullname", doctor.FullName);
                cmd.Parameters.AddWithValue("@Specialty", doctor.Specialty);
                cmd.Parameters.AddWithValue("@ContactNumber", doctor.ContactNumber);
                int i = cmd.ExecuteNonQuery();
                return Task.FromResult(doctor);
            }
        }



        public async Task<Response> UpdatePatient(Patient patient)
        {
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("update Patients set FullName = @FullName, DateOfBirth = @DateOfBirth, Gender = @Gender, ContactNumber = @ContactNumber where PatientId = @PatientID", connection);
                cmd.Parameters.AddWithValue("@PatientID", patient.PatientId);
                cmd.Parameters.AddWithValue("@FullName", patient.FullName);
                cmd.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                cmd.Parameters.AddWithValue("@Gender", patient.Gender);
                cmd.Parameters.AddWithValue("@ContactNumber", patient.ContactNumber);
                int roweffected = cmd.ExecuteNonQuery();
                if(roweffected ==0)
                {
                    throw new KeyNotFoundException("Patient record not found");
                }

            }
            return new Response { statuscode = HttpStatusCode.OK, message = "Patient record updated successfully" };
        }
    }
}

