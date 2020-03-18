using Model4You.Data.Common.Models;

namespace Model4You.Data.Models
{
    public class ContactFormData : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public bool Answered { get; set; }
    }
}