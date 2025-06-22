using MedProject_UI.Models;
using MongoDB.Driver;

namespace MedProject_UI.Services;

internal class MongoDbService
{
    private readonly IMongoCollection<Patient> _patients;

    public MongoDbService(string connectionString, string dbName, string collectionName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(dbName);
        _patients = database.GetCollection<Patient>(collectionName);
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
}