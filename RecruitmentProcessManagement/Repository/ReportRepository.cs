
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;

public class ReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _context;

    public ReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<PositionReportViewModel>> GetPositionWiseReport()
    {
        return await _context.Positions
            .Select(p => new PositionReportViewModel
            {
                PositionTitle = p.JobTitle,
                TotalLinkedCandidates = _context.Candidates.Count(c => c.CandidateID == p.LinkedCandidateID)
            })
            .ToListAsync();
    }

    public async Task<List<CollegeReportViewModel>> GetCollegeWiseReport()
    {
        return await _context.Candidates
            .GroupBy(c => c.CollegeName)
            .Select(g => new CollegeReportViewModel
            {
                CollegeName = g.Key,
                TotalCandidates = g.Count()
            })
            .ToListAsync();
    }

    public async Task<List<InterviewReportViewModel>> GetInterviewWiseReport()
    {
        return await _context.Interviews
            .GroupBy(i => i.InterviewDate.Date)
            .Select(g => new InterviewReportViewModel
            {
                InterviewDate = g.Key,
                TotalInterviews = g.Count(),
                CompletedInterviews = g.Count(i => i.Status == "Scheduled"),
                PendingInterviews = g.Count(i => i.Status != "Scheduled")
            })
            .OrderByDescending(r => r.InterviewDate)
            .ToListAsync();
    }
}
