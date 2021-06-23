using System.ComponentModel.DataAnnotations;

namespace StudentProgress.Core.Entities
{
    public enum ProgressStatus
    {
        [Display(Name = "Feedback with student")]
        FeedbackConversation = 1,   // Conversation held with student
        [Display(Name = "Feedback on delivered work")]
        OfflineFeedback = 2,        // Feedback given by mail, without a direct conversation with the student
        [Display(Name = "Evaluation")]
        Evaluation = 3,             // Intermediate grading, based on a sprint delivery
        [Display(Name = "Assessment (with student)")]
        Grading = 4,                // The end grade of the student, usually filled in at the assessment meeting
        [Display(Name = "Personal Note")]
        Note = 5,                   // Note by the teacher, can be grading of outcomes, or just comments / update of student status.
        [Display(Name = "Discussion with other teacher")]
        Intervision = 6,            // Note by the teacher, resulting fro discussion with other teacher(s)
        [Display(Name = "Other")]
        Other = 7                   // None of the above

    }
}
