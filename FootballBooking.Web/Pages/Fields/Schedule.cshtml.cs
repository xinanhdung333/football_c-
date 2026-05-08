using FootballBooking.BLL;
using FootballBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootballBooking.Web.Pages.Fields;

public class ScheduleModel : PageModel
{
    private readonly FieldManagementService _fieldService;

    public Field? Field { get; set; }

    public ScheduleModel(FieldManagementService fieldService)
    {
        _fieldService = fieldService;
    }

    public async Task<IActionResult> OnGetAsync(int fieldId)
    {
        var fields = await _fieldService.GetFieldsAsync();
        Field = fields?.FirstOrDefault(f => f.Id == fieldId);

        if (Field == null)
            return NotFound();

        return Page();
    }
}
