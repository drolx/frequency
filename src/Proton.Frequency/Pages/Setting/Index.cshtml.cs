using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Proton.Frequency.Pages.Setting;

public class Index : PageModel
{
    public IActionResult OnGet()
    {
        return Redirect("~/setting/config");
    }
}