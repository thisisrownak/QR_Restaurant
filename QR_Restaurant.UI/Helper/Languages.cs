using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Restaurant.UI.Helper
{
    public enum Languages
    {
        [Description("en")]
        [Display(Name = "English")]
        English,
        [Description("es")]
        [Display(Name = "Spanish")]
        Spanish, 
        [Description("de")]
        [Display(Name = "German")]
        German,
        [Description("ru")]
        [Display(Name = "Russian")]
        Russian,
        [Description("zh")]
        [Display(Name = "Chinese")]
        Chinese,
        [Description("ar")]
        [Display(Name = "Arabic")]
        Arabic,
        [Description("hi")]
        [Display(Name = "Hindi")]
        Hindi,
        [Description("pt")]
        [Display(Name = "Portuguse")]
        Portuguse,
        [Description("tr")]
        [Display(Name = "Turkish")]
        Turkish,
        [Description("ja")]
        [Display(Name = "Japanese")]
        Japanese,
        [Description("fr")]
        [Display(Name = "French")]
        French,
        [Description("pl")]
        [Display(Name = "Polish")]
        Polish,
        [Description("it")]
        [Display(Name = "İtalian")]
        İtalian
    }
}
