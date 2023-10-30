using System.ComponentModel.DataAnnotations;

namespace TrainingSocialMedia.ViewModels;

public class NewPostViewModel
{
    [Required]
    public string Content { get; set; } = "";
}