namespace Model4You.Services.Data.ModelService
{
    using System.Collections.Generic;

    using Model4You.Data.Models;

    public interface IModelService
    {
        IEnumerable<T> TakeSixModels<T>();
    }
}