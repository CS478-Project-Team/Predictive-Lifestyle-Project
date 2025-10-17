using System.ComponentModel.DataAnnotations;

public sealed class UserInputViewModel
{
    // Names should match Python config columns
    [Display(Name = "Category 1")]
    [Required]
    public string Col_Cat_1 { get; set; } = "Sex";

    [Display(Name = "Numeric 1")]
    [Required]
    public string Col_Num_1 { get; set; } = "Age";

    [Display(Name = "Numeric 2")]
    [Required]
    public string Col_Num_2 { get; set; } = "Weight";
    [Display(Name = "Numeric 3")]
    [Required]
    public string Col_Num_3 { get; set; } = "Height";
}
