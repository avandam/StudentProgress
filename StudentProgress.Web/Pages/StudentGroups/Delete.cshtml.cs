using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StudentProgress.Core.Entities;
using System.Threading.Tasks;

namespace StudentProgress.Web.Pages.StudentGroups
{
    public class DeleteModel : PageModel
    {
        private readonly ProgressContext _context;

        public DeleteModel(ProgressContext context)
        {
            _context = context;
        }

        [BindProperty]
        public StudentGroup Group { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Group = await _context.Groups.FirstOrDefaultAsync(m => m.Id == id);
            await _context.Entry(Group).Collection(group =>group.Students).LoadAsync();

            if (Group == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Group = await _context.Groups.FindAsync(id);


            if (Group != null && Group.Students.Count == 0)
            {
                _context.Groups.Remove(Group);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
