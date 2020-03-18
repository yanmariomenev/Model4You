using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model4You.Data.Common.Repositories;
using Model4You.Services.Mapping;

namespace Model4You.Services.Data.AdminServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Model4You.Data.Common.Models;
    using Model4You.Data.Models;

    public class ContactDataService : IContactDataService
    {
        private readonly IDeletableEntityRepository<ContactFormData> contactRep;

        public ContactDataService(IDeletableEntityRepository<ContactFormData> contactRep)
        {
            this.contactRep = contactRep;
        }

        public async Task<IEnumerable<T>> TakeAllUnAnswered<T>()
        {
            var unAnswered = contactRep.All().Where(x => x.IsDeleted == false);
            return await unAnswered.To<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> TakeAllAnswered<T>()
        {
            var answered = contactRep.AllWithDeleted().Where(x => x.IsDeleted == true);
            return await answered.To<T>().ToListAsync();
        }
    }
}