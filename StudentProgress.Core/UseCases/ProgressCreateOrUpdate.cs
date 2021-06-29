using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using StudentProgress.Core.Entities;

namespace StudentProgress.Core.UseCases
{
  public class ProgressCreateOrUpdate
  {
    private readonly ProgressContext _context;


    public ProgressCreateOrUpdate(ProgressContext context)
    {
      this._context = context;
    }

    public record MilestoneProgressCommand
    {
      public int? Id { get; set; }
      public int MilestoneId { get; set; }
      public Rating? PreviousRating { get; set; }
      public Rating? Rating { get; set; }
      public string? Comment { get; set; }
    }

    public record Command
    {
        public int? Id { get; set; }
        [Required] public int StudentId { get; set; }
        [Required] public int GroupId { get; set; }
        [Required] public Feeling Feeling { get; set; }
        [Required] public StatusInGroup StatusInGroup { get; set; }
        [Required] public bool WantsToSpeakToTeacher { get; set; }

        [Required] public ProgressStatus ProgressStatus { get; set; }
      public DateTime Date { get; set; }

      [Required]
      [DataType(DataType.Date)]
      public DateTime DateDate
      {
        get => Date.Date;
        set => Date = value.Date + DateTime.TimeOfDay;
      }

      [Required]
      [DataType(DataType.Time)]
      public DateTime DateTime
      {
        get => Date;
        set => Date = Date.Date + value.TimeOfDay;
      }
      [Display(Name = "Comments")] public string? Feedback { get; set; }
      public List<MilestoneProgressCommand> Milestones { get; set; } = new List<MilestoneProgressCommand>();
    }

    public async Task<Result> HandleAsync(Command command)
    {
      var student = Maybe<Student>.From(
          await _context.Students.FirstOrDefaultAsync(s => s.Id == command.StudentId)
      ).ToResult("Student does not exist");
      var group = Maybe<StudentGroup>.From(
          await _context.Groups.FirstOrDefaultAsync(g => g.Id == command.GroupId)
      ).ToResult("Group does not exist");
      var milestonesProgress = await GetMilestonesFrom(command.Milestones);
      var result = Result.Combine(student, group, milestonesProgress);

      if (result.IsFailure) return result;

      ProgressUpdate progressUpdate;
      var wantsToSpeakToTeacher = command.WantsToSpeakToTeacher;

      if (command.Id == null)
      {
        progressUpdate = new ProgressUpdate(
            student.Value,
            group.Value,
            command.Feedback,
            command.Feeling,
            command.Date,
            command.ProgressStatus);
        if (command.ProgressStatus == ProgressStatus.FeedbackConversation)
        {
            wantsToSpeakToTeacher = false;
        }
        progressUpdate.AddMilestones(milestonesProgress.Value);
        await _context.ProgressUpdates.AddAsync(progressUpdate);
      }
      else
      {
        progressUpdate = await _context
            .ProgressUpdates
            .Include(p => p.MilestonesProgress)
            .ThenInclude(mp => mp.Milestone)
            .FirstOrDefaultAsync(p => p.Id == command.Id);

        progressUpdate.Update(command.Feeling, command.Date, command.Feedback, command.ProgressStatus);
        UpdateMilestoneProgresses(progressUpdate, milestonesProgress.Value);
      }

      var studentStatuses = await _context.StudentStatuses
          .Where(u => u.GroupId == group.Value.Id && u.StudentId == student.Value.Id)
          .ToListAsync();

      StudentStatus studentStatus;
      if (studentStatuses.Count > 0)
      {
          studentStatus = studentStatuses.First();
          studentStatus.Update(command.StatusInGroup, wantsToSpeakToTeacher);
      }
      else
      {
          studentStatus = new StudentStatus(student.Value, group.Value, command.StatusInGroup, wantsToSpeakToTeacher);
          await _context.StudentStatuses.AddAsync(studentStatus);
      }

      await _context.SaveChangesAsync();
      return Result.Success();
    }

    private async Task<Result<List<MilestoneProgress>>> GetMilestonesFrom(List<MilestoneProgressCommand> milestonesProgress)
    {
      var milestonesToAdd = milestonesProgress.Where(m => m.Rating != null).ToList();
      var milestoneIds = milestonesToAdd.Select(m => m.MilestoneId);
      var milestones = await _context.Milestones.Where(m => milestoneIds.Contains(m.Id)).ToListAsync();
      var milestoneProgressWithRating = milestones
          .Select(m =>
          {
            var milestone = milestonesToAdd.FirstOrDefault(mp => mp.MilestoneId == m.Id);
            return new MilestoneProgress(milestone?.Rating ?? Rating.Undefined, m, milestone?.Comment);
          })
          .ToList();

      return Result.Success(milestoneProgressWithRating);
    }

    private void UpdateMilestoneProgresses(ProgressUpdate progressUpdate, List<MilestoneProgress> commandMilestoneProgresses) {
        var result = progressUpdate.UpdateMilestoneProgressesWith(commandMilestoneProgresses);
        _context.MilestoneProgresses.RemoveRange(result.RemovedMilestoneProgresses);
    }
  }
}