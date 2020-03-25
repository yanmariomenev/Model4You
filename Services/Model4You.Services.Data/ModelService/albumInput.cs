using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Model4You.Services.Data.ModelService
{
    public class albumInput
    {
        public albumInput()
        {
            UserImages = new List<IFormFile>();
        }

        [DataType(DataType.Upload)]
        public ICollection<IFormFile> UserImages { get; set; }
    }
}