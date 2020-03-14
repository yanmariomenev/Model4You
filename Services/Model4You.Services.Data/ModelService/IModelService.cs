using System.Collections.Generic;
using Model4You.Data.Models;

namespace Model4You.Services.Data.ModelService
{
    public interface IModelService
    {
        IEnumerable<T> TakeSixModels<T>();
    }
}