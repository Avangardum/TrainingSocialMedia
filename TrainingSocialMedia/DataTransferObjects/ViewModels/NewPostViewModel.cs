using System.ComponentModel.DataAnnotations;

namespace TrainingSocialMedia.DataTransferObjects.ViewModels;

public class NewPostViewModel
{
    [Required]
    public string Content { get; set; } = "";
}