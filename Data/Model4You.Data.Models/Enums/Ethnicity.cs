using System.ComponentModel.DataAnnotations;

namespace Model4You.Data.Models.Enums
{
    public enum Ethnicity
    {
        //[Display(Name = "None")]
        //None = 0,
        [Display(Name = "White")]
        White = 1,
        [Display(Name = "Black African Roots")]
        BlackAfricanRoots = 2,
        [Display(Name = "Latino Hispanic")]
        LatinoHispanic = 3,
        [Display(Name = "Native American")]
        NativeAmerican = 4,
        [Display(Name = "Indian Pakistani")]
        IndianPakistani = 5,
        [Display(Name = "Middle Eastern")]
        MiddleEastern = 6,
        [Display(Name = "Chinese")]
        Chinese = 7,
        [Display(Name = "Japanese")]
        Japanese = 8,
        [Display(Name = "Korean")]
        Korean = 9,
        [Display(Name = "South Asia")]
        SouthAsia = 10,
        [Display(Name = "Other")]
        Other = 11,
    }
}