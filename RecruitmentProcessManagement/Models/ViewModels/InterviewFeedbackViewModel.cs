using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models.ViewModels
{
    public class InterviewFeedbackViewModel
    {
        public int InterviewRoundID { get; set; }
        public int InterviewID { get; set; }

        public int RoundNumber { get; set; }

        [Required(ErrorMessage = "Feedback is required.")]
        [StringLength(1000, ErrorMessage = "Feedback cannot exceed 1000 characters.")]
        public string Feedback { get; set; }

        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
        public int Rating { get; set; }
    }
}
