using System.Security.Cryptography.X509Certificates;

namespace Model4You.Services.Data.ModelService
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Model4You.Data.Common.Repositories;
    using Model4You.Data.Models;
    using Model4You.Services.Mapping;

    public class ModelService : IModelService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> appRepository;

        public ModelService(IDeletableEntityRepository<ApplicationUser> appRepository)
        {
            this.appRepository = appRepository;
        }

        //public IEnumerable<T> GetAll<T>()
        //{
        //    var user = this.appRepository.All()
        //        .Where(x => x.ModelInformation != null).Take(6);
        //    return user.To<T>().ToList();
        //}

        public IEnumerable<T> TakeSixModels<T>()
        {
            var user = this.appRepository.All()
                .Where(x => x.ModelInformation != null).Take(6);
            return user.To<T>().ToList();
        }
    }
}