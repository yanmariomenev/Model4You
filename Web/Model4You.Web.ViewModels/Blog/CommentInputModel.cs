using System.ComponentModel.DataAnnotations;
using Model4You.Services.Mapping;

namespace Model4You.Web.ViewModels.Blog
{
    public class CommentInputModel
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Content { get; set; }

        public int BlogContentId { get; set; }

        public int BlogId { get; set; }
    }
}