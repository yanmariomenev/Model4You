using System.Threading.Tasks;

namespace Model4You.Services.Data.ContactFormService
{
    public interface IContactFormService
    {
        Task Create(string name, string email, string subject, string message);
    }
}