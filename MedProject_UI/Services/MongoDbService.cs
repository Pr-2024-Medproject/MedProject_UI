using MedProject_UI.Models;
using MongoDB.Driver;

namespace MedProject_UI.Services;

internal class MongoDbService
{
    private readonly IMongoCollection<Patient> _patients;
    private readonly IMongoCollection<Doctor> _doctors;

    public MongoDbService(string connectionString, string dbName)
    {
        var config = AppConfig.Load();

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(dbName);

        _patients = database.GetCollection<Patient>(config.PatientsCollection);
        _doctors = database.GetCollection<Doctor>(config.DoctorsCollection);
    }

    public async Task<List<Patient>> GetAllPatientsAsync()
    {
        return await _patients.Find(_ => true).ToListAsync();
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

    public async Task AddDoctorAsync(Doctor doctor)
    {
        await _doctors.InsertOneAsync(doctor);
    }

    public async Task UpdateDoctorAsync(Doctor doctor)
    {
        await _doctors.ReplaceOneAsync(d => d.Id == doctor.Id, doctor);
    }

    public async Task DeleteDoctorAsync(string id)
    {
        await _doctors.DeleteOneAsync(d => d.Id == id);
    }

}