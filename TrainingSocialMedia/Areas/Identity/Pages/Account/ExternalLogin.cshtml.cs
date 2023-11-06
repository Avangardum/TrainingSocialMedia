using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrainingSocialMedia.Areas.Identity.Pages.Account;

public class ExternalLoginModel : PageModel
{
    public IActionResult OnGet() => NotFound();
}