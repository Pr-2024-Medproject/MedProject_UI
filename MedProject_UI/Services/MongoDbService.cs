using MedProject_UI.Models;
using MongoDB.Driver;

namespace MedProject_UI.Services;

internal class MongoDbService : IDisposable
{
    private readonly IMongoCollection<Patient> _patients;
    private readonly IMongoCollection<Doctor> _doctors;
    private readonly IMongoDatabase _database;
    private readonly MongoClient _client;
    private bool _disposed = false;

    public MongoDbService(string connectionString, string dbName)
    {
        var config = AppConfig.Load();

        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(dbName);

        _patients = _database.GetCollection<Patient>(config.PatientsCollection);
        _doctors = _database.GetCollection<Doctor>(config.DoctorsCollection);
    }

    public async Task<List<Patient>> GetAllPatientsAsync()
    {
        return await _patients.Find(_ => true).ToListAsync();
    }

    public async Task<List<Patient>> GetPatientsByDoctorIdAsync(string doctorId)
    {
        var filter = Builders<Patient>.Filter.Eq(p => p.DoctorId, doctorId);
        return await _patients.Find(filter).ToListAsync();
    }

    public async Task<Patient?> GetPatientByIdAsync(string id)
    {
        return await _patients.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task AddPatientAsync(Patient patient)
    {
        await _patients.InsertOneAsync(patient);
    }

    public async Task InsertPatientAsync(Patient patient)
    {
        await _patients.InsertOneAsync(patient);
    }

    public async Task UpdatePatientAsync(Patient patient)
    {
        await _patients.ReplaceOneAsync(p => p.Id == patient.Id, patient);
    }

    public async Task InsertOrUpdatePatientAsync(Patient patient)
    {
        if (string.IsNullOrEmpty(patient.Id))
            await InsertPatientAsync(patient);
        else
            await UpdatePatientAsync(patient);
    }

    public async Task DeletePatientAsync(string id)
    {
        await _patients.DeleteOneAsync(p => p.Id == id);
    }

    public async Task<List<Doctor>> GetAllDoctorsAsync()
    {
        return await _doctors.Find(_ => true).ToListAsync();
    }

    public async Task<Doctor?> GetDoctorByIdAsync(string id)
    {
        return await _doctors.Find(d => d.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Doctor?> GetDoctorByUsernameAsync(string username)
    {
        return await _doctors.Find(d => d.Username == username).FirstOrDefaultAsync();
    }

    public async Task<Doctor?> GetDoctorByEmailAsync(string email)
    {
        return await _doctors.Find(d => d.Email == email).FirstOrDefaultAsync();
    }

    public async Task<bool> CheckDoctorExistsByEmailAsync(string email)
    {
        var filter = Builders<Doctor>.Filter.Eq(d => d.Email, email);
        var result = await _doctors.Find(filter).FirstOrDefaultAsync();
        return result != null;
    }

    public async Task AddDoctorAsync(Doctor doctor)
    {
        await _doctors.InsertOneAsync(doctor);
    }

    public async Task UpdateDoctorAsync(Doctor doctor)
    {
        await _doctors.ReplaceOneAsync(d => d.Id == doctor.Id, doctor);
    }

    public async Task<bool> UpdateDoctorScheduleAsync(string doctorId, List<WorkPeriod> schedule)
    {
        var filter = Builders<Doctor>.Filter.Eq(d => d.Id, doctorId);
        var update = Builders<Doctor>.Update.Set(d => d.WorkSchedule, schedule);

        var result = await _doctors.UpdateOneAsync(filter, update);

        return result.ModifiedCount > 0;
    }

    public async Task DeleteDoctorAsync(string id)
    {
        await _doctors.DeleteOneAsync(d => d.Id == id);
    }

    // Dispose pattern
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                //_client?.Cluster.Dispose(); 
            }

            _disposed = true;
        }
    }
}