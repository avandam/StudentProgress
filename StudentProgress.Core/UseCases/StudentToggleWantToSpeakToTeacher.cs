using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using StudentProgress.Core.Entities;

namespace StudentProgress.Core.UseCases
{
    public class StudentToggleWantToSpeakToTeacher
    {
        private readonly ProgressContext _context;

        public StudentToggleWantToSpeakToTeacher(ProgressContext context)
        {
            _context = context;
        }

        public async Task<Result> HandleAsync(Command command)
        {
            var studentStatus = Maybe<StudentStatus>.From(await _context.StudentStatuses.FindAsync(command.Id))
                .ToResult("StudentStatus does not exist");

            if (studentStatus.IsFailure)
            {
                return Result.Failure("StudentStatus does not exist");
            }

            return await studentStatus.Value
                .Update(!studentStatus.Value.WantsToSpeakToTeacher)
                .Tap(async () => await _context.SaveChangesAsync());
        }

        public record Command
        {
            public int Id { get; set; }
            public int GroupId { get; set; }
        }
    }
}