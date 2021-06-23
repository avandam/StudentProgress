using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using StudentProgress.Core.Entities;

namespace StudentProgress.Core.UseCases
{
    public class ProgressTagDelete
    {
        private readonly ProgressContext _db;

        public ProgressTagDelete(ProgressContext db)
        {
            _db = db;
        }

        public async Task<Result> HandleAsync(Command command)
        {
            var progressTag = await _db.ProgressTags.FindAsync(command.Id);

            _db.ProgressTags.Remove(progressTag);

            await _db.SaveChangesAsync();
            return Result.Success();
        }

        public record Command
        {
            public int Id { get; set; }
        }
    }
}