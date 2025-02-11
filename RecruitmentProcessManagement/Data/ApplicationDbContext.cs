using System.Reflection.Emit;
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
        public DbSet<Interviewer> Interviewers { get; set; }
        public DbSet<InterviewerSkill> InterviewerSkills { get; set; }
        public DbSet<InterviewRoundInterviewer> InterviewRoundInterviewers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCandidate> EventsCandidates { get; set; }
        public DbSet<EventInterviewer> EventsInterviewers { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.LinkedCandidate)
                .WithMany()
                .HasForeignKey(p => p.LinkedCandidateID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<InterviewFeedback>()
                .HasOne(ifb => ifb.InterviewRound)
                .WithMany()
                .HasForeignKey(ifb => ifb.InterviewRoundID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PositionSkill>()
                .HasOne(ps => ps.Position)
                .WithMany(p => p.PositionSkills)
                .HasForeignKey(ps => ps.PositionID);

            modelBuilder.Entity<PositionSkill>()
                .HasOne(ps => ps.Skill)
                .WithMany(s => s.PositionSkills)
                .HasForeignKey(ps => ps.SkillID);

            modelBuilder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Candidate)
                .WithMany(c => c.CandidateSkills)
                .HasForeignKey(cs => cs.CandidateID);

            modelBuilder.Entity<CandidateSkill>()
                .HasOne(cs => cs.Skill)
                .WithMany(s => s.CandidateSkills)
                .HasForeignKey(cs => cs.SkillID);

            modelBuilder.Entity<CandidateDocument>()
                .HasOne(cd => cd.Candidate)
                .WithMany(c => c.CandidateDocuments)
                .HasForeignKey(cd => cd.CandidateID);

            modelBuilder.Entity<CandidateReview>()
                .HasOne(cr => cr.Candidate)
                .WithMany(c => c.CandidateReviews)
                .HasForeignKey(cr => cr.CandidateID);

            modelBuilder.Entity<CandidateReview>()
                .HasOne(cr => cr.Position)
                .WithMany(p => p.CandidateReviews)
                .HasForeignKey(cr => cr.PositionID);

            modelBuilder.Entity<CandidateReview>()
                .HasOne(cr => cr.Reviewer)
                .WithMany()
                .HasForeignKey(cr => cr.ReviewerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Candidate)
                .WithMany(c => c.Interviews)
                .HasForeignKey(i => i.CandidateID);

            modelBuilder.Entity<Interview>()
                .HasOne(i => i.Position)
                .WithMany(p => p.Interviews)
                .HasForeignKey(i => i.PositionID);

            modelBuilder.Entity<DocumentVerification>()
                .HasOne(dv => dv.Candidate)
                .WithMany(c => c.DocumentVerifications)
                .HasForeignKey(dv => dv.CandidateID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DocumentVerification>()
                .HasOne(dv => dv.VerifiedByUser)
                .WithMany()
                .HasForeignKey(dv => dv.VerifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CandidateStatusHistory>()
                .HasOne(csh => csh.Candidate)
                .WithMany(c => c.CandidateStatusHistories)
                .HasForeignKey(csh => csh.CandidateID);

            modelBuilder.Entity<CandidateStatusHistory>()
                .HasOne(csh => csh.ChangedByUser)
                .WithMany()
                .HasForeignKey(csh => csh.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventOrganizer)
                .WithMany()
                .HasForeignKey(e => e.EventOrganizerID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InterviewFeedback>()
                .HasOne(ifb => ifb.Interviewer)
                .WithMany()
                .HasForeignKey(ifb => ifb.InterviewerID)
                .OnDelete(DeleteBehavior.Restrict);

            // Ensuring the AspNetUser ID for Identity is treated as a string
            modelBuilder.Entity<IdentityUser>().Property(u => u.Id).HasColumnType("nvarchar(450)");
        }

    }
}

