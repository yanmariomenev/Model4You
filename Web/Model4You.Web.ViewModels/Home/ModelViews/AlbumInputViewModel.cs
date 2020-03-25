namespace Model4You.Web.ViewModels.ModelViews
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class AlbumInputViewModel
    {
        public AlbumInputViewModel()
        {
            UserImages = new List<IFormFile>();
        }

        [DataType(DataType.Upload)]
        public ICollection<IFormFile> UserImages { get; set; }
    }
}