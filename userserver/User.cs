using System.ComponentModel.DataAnnotations;

namespace WebApplication1;

public class User
{
    public int Id { get; set; } = 0;
    
    [MinLength(1, ErrorMessage = "Name can't be empty")]
    [Required(ErrorMessage = "User name required")]
    public string Name { get; set; } = "";
    
    [Range(1, 100, ErrorMessage = "Age must be a number in range 1...100")]
    [Required(ErrorMessage = "User age required")]
    public int Age { get; set; }
}