using System.Collections.Generic;
using Model4You.Data.Models;
using Model4You.Services.Mapping;
using Model4You.Web.ViewModels.Model;
using Model4You.Web.ViewModels.ModelViews;

namespace Model4You.Web.ViewModels.BindingModels
{
    public class AlbumBindingViewModel
    {
        public IEnumerable<AlbumViewModel> AlbumViewModel { get; set; }

        public AlbumInputViewModel AlbumInputViewModel { get; set; }

        public int ImageCount { get; set; }
    }
}