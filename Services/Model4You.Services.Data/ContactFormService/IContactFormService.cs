namespace Model4You.Services.Data.ContactFormService
{
    using System.Threading.Tasks;

    public interface IContactFormService
    {
        Task Create(string name, string email, string subject, string message);
    }
}