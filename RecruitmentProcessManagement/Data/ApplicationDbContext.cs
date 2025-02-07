using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Position> Positions { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<PositionSkill> PositionSkills { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateSkill> CandidateSkills { get; set; }
        public DbSet<CandidateDocument> CandidateDocuments { get; set; }
        public DbSet<CandidateReview> CandidateReviews { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<InterviewRound> InterviewRounds { get; set; }
        public DbSet<InterviewFeedback> InterviewFeedbacks { get; set; }
        public DbSet<DocumentVerification> DocumentVerifications { get; set; }
        public DbSet<FinalSelection> FinalSelections { get; set; }
        public DbSet<CandidateStatusHistory> CandidateStatusHistories { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Defining relationships explicitly for Identity references (IdentityUser Tables)
            modelBuilder.Entity<CandidateReview>()
                .HasOne(cr => cr.Reviewer)
                .WithMany()
                .HasForeignKey(cr => cr.ReviewerID);

            //modelBuilder.Entity<Interview>()
            //    .HasOne(i => i.Interviewer)
            //    .WithMany()
            //    .HasForeignKey(i => i.InterviewerID);

            modelBuilder.Entity<InterviewFeedback>()
                .HasOne(ifb => ifb.InterviewRound)
                .WithMany()
                .HasForeignKey(ifb => ifb.InterviewRoundID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DocumentVerification>()
                .HasOne(dv => dv.VerifiedByUser)
                .WithMany()
                .HasForeignKey(dv => dv.VerifiedBy);

            modelBuilder.Entity<CandidateStatusHistory>()
                .HasOne(csh => csh.ChangedByUser)
                .WithMany()
                .HasForeignKey(csh => csh.ChangedBy);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID);
                
            // Ensuring the AspNetUser ID for Identity is treated as a string
            modelBuilder.Entity<IdentityUser>().Property(u => u.Id).HasColumnType("nvarchar(450)");
        }
    }
}
