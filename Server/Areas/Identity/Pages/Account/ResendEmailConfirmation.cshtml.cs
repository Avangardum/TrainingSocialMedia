using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrainingSocialMedia.Server.Areas.Identity.Pages.Account;

public class ResendEmailConfirmationModel : PageModel
{
    public IActionResult OnGetAsync()
    {
        return NotFound();
    }
}