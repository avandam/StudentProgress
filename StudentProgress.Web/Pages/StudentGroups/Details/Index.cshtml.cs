﻿using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudentProgress.Core.UseCases;
using System.Threading.Tasks;
using StudentProgress.Core.Entities;

namespace StudentProgress.Web.Pages.StudentGroups.Details
{
    public class IndexModel : PageModel
    {
        private readonly StudentGroupGetDetails _useCase;
        private readonly MilestonesUpdateLearningOutcome _milestonesUpdateUseCase;
        private readonly StudentToggleWantToSpeakToTeacher _studentToggleWantToSpeakToTeacher;

        public IndexModel(IDbConnection connection, ProgressContext context)
        {
            _useCase = new StudentGroupGetDetails(connection, context);
            _milestonesUpdateUseCase = new MilestonesUpdateLearningOutcome(context);
            _studentToggleWantToSpeakToTeacher = new StudentToggleWantToSpeakToTeacher(context);
        }

        public bool IsSortedOnLastFeedback { get; set; }
        public bool IsSortedOnLastSpoken { get; set; }
        public StudentGroupGetDetails.Response StudentGroup { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string? sort)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentGroup = await _useCase.HandleAsync(new StudentGroupGetDetails.Request((int)id));

            if (StudentGroup == null)
            {
                return NotFound();
            }

            if (sort == "last-spoken")
            {
                IsSortedOnLastSpoken = true;
                IsSortedOnLastFeedback = false;
                StudentGroup.Students =
                    StudentGroup.Students.OrderByDescending(s => s.StatusInGroup != StatusInGroup.Stopped && s.StatusInGroup != StatusInGroup.Inactive).ThenBy(s => s.LastSpokenWithStudentDate).ToList();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostToggleWantsToSpeakToTeacherAsync(
            StudentToggleWantToSpeakToTeacher.Command command)
        {
            var result = await _studentToggleWantToSpeakToTeacher.HandleAsync(command);

            if (result.IsFailure)
            {
                ModelState.AddModelError("StudentStatusUpdate", result.Error);
            }

            return await OnGetAsync(command.GroupId, null);
        }

        public async Task<IActionResult> OnPostUpdateMultipleMilestonesAsync(MilestonesUpdateLearningOutcome.Command command)
        {
            var result = await _milestonesUpdateUseCase.HandleAsync(command);

            if (result.IsFailure)
            {
                ModelState.AddModelError("MilestoneSummary", result.Error);
            }
            
            return await OnGetAsync(command.GroupId, null);
        }
    }
}
