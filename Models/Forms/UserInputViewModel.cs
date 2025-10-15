using System.ComponentModel.DataAnnotations;

public sealed class UserInputViewModel
{
    // Names should match Python config columns
    [Display(Name = "Numeric 1")]
    [Required]
    public string Col_Num_1 { get; set; } = "";  // we allow commas; Python coerces

    [Display(Name = "Numeric 2")]
    [Required]
    public string Col_Num_2 { get; set; } = "";

    [Display(Name = "Category 1")]
    [Required]
    public string Col_Cat_1 { get; set; } = "";

    [Display(Name = "Category 2")]
    [Required]
    public string Col_Cat_2 { get; set; } = "";
}
