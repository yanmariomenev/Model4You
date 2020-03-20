using System.Threading.Tasks;

namespace Model4You.Services.Data.ModelService
{
    using System.Collections.Generic;

    using Model4You.Data.Models;

    public interface IModelService
    {
        Task<IEnumerable<T>> TakeSixModels<T>();

        Task<IEnumerable<T>> TakeAllModels<T>();

        Task<T> GetModelById<T>(string modelId);
    }
}