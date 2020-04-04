using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model4You.Data.Common.Models;
using Model4You.Data.Common.Repositories;
using Model4You.Data.Models;
using Model4You.Data.Models.Enums;
using Model4You.Services.Mapping;

namespace Model4You.Services.Data.SearchService
{
    public class SearchService : ISearchService
    {
        private readonly IDeletableEntityRepository<ModelInformation> modelInfoRepo;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepo;

        public SearchService(
            IDeletableEntityRepository<ModelInformation> modelInfoRepo,
            IDeletableEntityRepository<ApplicationUser> userRepo)
        {
            this.modelInfoRepo = modelInfoRepo;
            this.userRepo = userRepo;
        }

        public async Task<ICollection<T>> SearchResult<T>
            (string country, string city, string gender, int age, int to)
        {
            Enum.TryParse(gender, out Gender result);
            var getUserIdForSearchQuery = this.userRepo
                .All()
                .Where
                (x => x.ModelInformation.Country == country
                      && x.ModelInformation.City == city
                      && x.ModelInformation.Age >= age
                      && x.ModelInformation.Age <= to
                      && x.ModelInformation.Gender == result)
                .OrderByDescending(x => x.CreatedOn);
            return await getUserIdForSearchQuery.To<T>().ToListAsync();
        }
    }
}