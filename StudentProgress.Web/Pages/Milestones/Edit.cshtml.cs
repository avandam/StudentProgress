﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentProgress.Core.Entities;
using StudentProgress.Core.UseCases;

namespace StudentProgress.Web.Pages.Milestones
{
    public class Edit : PageModel
    {
        private readonly ProgressContext _context;
        private readonly MilestoneUpdate _useCase;

        public Edit(ProgressContext context)
        {
            _context = context;
            _useCase = new MilestoneUpdate(context);
        }

        [BindProperty] public MilestoneUpdate.Command Command { get; set; }

        public async Task OnGetAsync(int id)
        {
            var milestone = await _context.Milestones.FindAsync(id);
            Command = new MilestoneUpdate.Command
            {
                Id = id,
                Artefact = milestone.Artefact,
                LearningOutcome = milestone.LearningOutcome
            };
        }

        public async Task<IActionResult> OnPostAsync(int id, int groupId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _useCase.HandleAsync(Command);

            if (result.IsFailure)
            {
                ModelState.AddModelError("Summary", result.Error);
                return Page();
            }

            return RedirectToPage("/StudentGroups/Details/Index", new {Id = groupId});
        }
    }
}