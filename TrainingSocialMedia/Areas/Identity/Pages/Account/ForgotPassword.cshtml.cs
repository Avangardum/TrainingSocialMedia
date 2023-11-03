using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrainingSocialMedia.Areas.Identity.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    public IActionResult OnGet() => NotFound();
}