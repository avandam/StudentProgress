using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using StudentProgress.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentProgress.Core.UseCases
{
    public class ProgressGetForCreateOrUpdate
    {
        public record Query
        {
            public int? Id { get; set; }
            public int GroupId { get; set; }
            public int StudentId { get; set; }
        }

        public record Response
        {
            public Student Student { get; set; }
            public StudentGroup Group { get; set; }
            public List<Milestone> Milestones { get; set; }
            public ProgressCreateOrUpdate.Command Command { get; set; }

            public Response(Student student, StudentGroup group, List<Milestone> milestones,
                ProgressCreateOrUpdate.Command command)
            {
                Student = student;
                Group = group;
                Milestones = milestones;
                Command = command;
            }
        }

        private readonly ProgressContext _context;

        public ProgressGetForCreateOrUpdate(ProgressContext context)
        {
            _context = context;
        }

        public async Task<Result<Response>> HandleAsync(Query query)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == query.StudentId);
            var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == query.GroupId);

            var milestones = _context.Milestones
                .Where(m => m.StudentGroup.Id == query.GroupId)
                .OrderBy(m => m.LearningOutcome)
                .ThenBy(m => m.Artefact)
                .ToList();
            var progressUpdate = await _context.ProgressUpdates
                .Include(p => p.MilestonesProgress)
                .ThenInclude(m => m.Milestone)
                .FirstOrDefaultAsync(p => p.Id == (query.Id ?? 0));

            var studentMilestoneProgresses = await _context.MilestoneProgresses
                .Include(mp => mp.Milestone)
                .Where(mp =>
                    mp.ProgressUpdate.StudentId == query.StudentId && mp.Milestone.StudentGroup.Id == query.GroupId)
                .OrderByDescending(mp => mp.ProgressUpdate.Date)
                .ToListAsync();

            var studentStatuses = await _context.StudentStatuses
                .Where(u => u.GroupId == query.GroupId && u.StudentId == query.StudentId)
                .ToListAsync();
            var studentStatus = studentStatuses.Count > 0 ? studentStatuses.First().StatusInGroup : StatusInGroup.Active;

            if (student == null || group == null || (query.Id != null && progressUpdate == null))
            {
                return Result.Failure<Response>(
                    "Either group and/or student doesn't exist, or you're trying to access a non-existing progress update");
            }


            var command = new ProgressCreateOrUpdate.Command
            {
                Date = progressUpdate?.Date ?? DateTime.Now,
                Feedback = progressUpdate?.Feedback,
                GroupId = group.Id,
                StudentId = student.Id,
                Id = progressUpdate?.Id,
                Feeling = progressUpdate?.ProgressFeeling ?? Feeling.Neutral,
                StatusInGroup = studentStatus,
                ProgressStatus = progressUpdate?.ProgressStatus ?? ProgressStatus.FeedbackConversation,
                Milestones = milestones.Select(milestone =>
                    {
                        var milestoneProgresses =
                            studentMilestoneProgresses.Where(mp => mp.Milestone.Id == milestone.Id && mp.CreatedDate < (progressUpdate?.CreatedDate ?? DateTime.Now)).ToList();
                        var latestProgress = milestoneProgresses.FirstOrDefault();

                        var milestoneProgress =
                            progressUpdate?.MilestonesProgress.FirstOrDefault(pu => milestone.Id == pu.Milestone.Id);
                        return new ProgressCreateOrUpdate.MilestoneProgressCommand
                        {
                            Rating = milestoneProgress?.Rating,
                            PreviousRating = latestProgress?.Rating,
                            Comment = milestoneProgress?.Comment,
                            MilestoneId = milestone.Id,
                            Id = milestoneProgress?.Id
                        };
                    })
                    .ToList()
            };

            return Result.Success(new Response(student!, group!, milestones, command));
        }
    }
}