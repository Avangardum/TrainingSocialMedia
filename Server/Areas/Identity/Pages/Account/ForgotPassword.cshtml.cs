using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrainingSocialMedia.Server.Areas.Identity.Pages.Account;

public class ForgotPasswordModel : PageModel
{
    public IActionResult OnGet()
    {
        return NotFound();
    }
}