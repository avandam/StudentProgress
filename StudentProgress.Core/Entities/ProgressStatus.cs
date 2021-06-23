using System.ComponentModel.DataAnnotations;

namespace StudentProgress.Core.Entities
{
    public enum ProgressStatus
    {
        [Display(Name = "Unknown")]
        Unknown = 0,                 // Unknown, only for backwards compatibility, will not be used in new statusses
        [Display(Name = "Feedback")]
        FeedbackConversation = 1,   // Conversation held with student
        [Display(Name = "Reply")]
        OfflineFeedback = 2,        // Feedback given by mail, without a direct conversation with the student
        [Display(Name = "Evaluation")]
        Evaluation = 3,             // Intermediate grading, based on a sprint delivery
        [Display(Name = "Grading")]
        Grading = 4,                // The end grade of the student, usually filled in at the assessment meeting
        [Display(Name = "Note")]
        Note = 5,                   // Note by the teacher, can be grading of outcomes, or just comments / update of student status.
        [Display(Name = "Intervisie")]
        Intervision = 6,            // Note by the teacher, resulting fro discussion with other teacher(s)
        [Display(Name = "Status update")]
        StatusUpdate = 7,          // The status of the student is updated (used when a student stops or gets inactive)
        [Display(Name = "Other")]
        Other = 8,                  // None of the above
    }
}
