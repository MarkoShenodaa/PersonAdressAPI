namespace PersonAdressAPI.Services.Pepole
{
    public interface IPerson : IAPIService<Person>
    {
        Task<List<Person>> GetPersonByName(string name);

        Task<List<Person>> AddLoginToPerson(int personID,AuthenticateRequest request);

        Task<AuthenticateResponse> GetPersonByLoginData(AuthenticateRequest request);
    }
}