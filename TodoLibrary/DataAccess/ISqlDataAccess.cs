
namespace TodoLibrary.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        Task SavedData<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}