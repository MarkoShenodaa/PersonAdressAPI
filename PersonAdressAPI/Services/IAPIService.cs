namespace PersonAdressAPI.Services;

public interface IAPIService<TModel>
{
    Task<List<TModel>> GetAllModel();

    Task<TModel> GetModelByID(int id);

    Task<List<TModel>> UpdateModel(TModel model);

    Task<List<TModel>> DeleteModel(int id);

    Task<List<TModel>> AddModel(TModel model);
}