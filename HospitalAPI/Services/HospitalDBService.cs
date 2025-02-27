using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text.Json;
using HospitalAPI.Models;
using HospitalAPI.Services.Interfaces;


namespace HospitalAPI.Services
{
    public class HospitalDBService : IHospitalDBService
    {
        private readonly string _connection;

        private readonly ILogger<HospitalDBService> _logger;
        private readonly string _jsonFilePath = "D:\\Mounika_.NET\\API\\HospitalAPI\\HospitalAPI\\HospitalAPI\\Data\\Userdetails.json";

        public HospitalDBService(IConfiguration configuration, ILogger<HospitalDBService> logger )
        {
            _connection = configuration.GetConnectionString("HospitalConnection");
            _logger = logger;
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
                _logger.LogInformation("Connection opened successfully");
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
                _logger.LogInformation("Data read successfully");
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

        // UpdateDoctor method using stored procedure
        public async Task<Response> UpdateDoctor(Doctor doctor)
        {
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("UpdateDoctordetails", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorID", doctor.DoctorId);
                cmd.Parameters.AddWithValue("@FullName", doctor.FullName);
                cmd.Parameters.AddWithValue("@Specialty", doctor.Specialty);
                cmd.Parameters.AddWithValue("@ContactNumber", doctor.ContactNumber);
                int roweffected = cmd.ExecuteNonQuery();
                if (roweffected == 0)
                {
                    throw new KeyNotFoundException("Doctor record not found");
                }
            }
            return new Response { statuscode = HttpStatusCode.OK, message = "Doctor record updated successfully" };
        }

        public Task<User> ValidateUser(string username)
        {
            // connect to the json file

            if (File.Exists(_jsonFilePath))
            {
                // read the json file
                var json = File.ReadAllText(_jsonFilePath);
                // deserialize the json file
                var users = JsonSerializer.Deserialize<List<User>>(json);
                // check the username exists
                var user = users.FirstOrDefault(x => x.Username == username);
                if (user != null)
                {
                    User validateuser = new User()
                    {
                        Username = user.Username,
                        Password = user.Password,
                    };
                    return Task.FromResult(validateuser);
                }
            }
            else
            {
                _logger.LogError("File not found");
            }
            return Task.FromResult<User>(null);

        }

        public Task<User> AddUserdetails(User user)
        {
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("insert into Userdata values(@UserName, @Password)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.ExecuteNonQuery();
                return Task.FromResult(user);
            }
        }
        public Task<User> SqlValidateUser(string username)
        {
            // connect to the sql server
            using (var connection = new SqlConnection(_connection))
            {
                connection.Open();
                // sql command 
                SqlCommand cmd = new SqlCommand("select * from Userdata where Username = @Username", connection);
                cmd.Parameters.AddWithValue("@Username", username);
                // execute the command
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        User validateuser = new User()
                        {
                            Username = reader.GetString(0),
                            Password = reader.GetString(1),
                        };
                        return Task.FromResult(validateuser);
                    }
                    else
                    {
                        _logger.LogError("User not found");
                        return Task.FromResult<User>(null);
                    }
                }

            }

        }

       
    }
}

