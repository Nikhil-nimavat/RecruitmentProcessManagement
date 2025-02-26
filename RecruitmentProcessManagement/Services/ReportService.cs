
public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<List<PositionReportViewModel>> GetPositionWiseReport()
    {
        return await _reportRepository.GetPositionWiseReport();
    }

    public async Task<List<CollegeReportViewModel>> GetCollegeWiseReport()
    {
        return await _reportRepository.GetCollegeWiseReport();
    }

    public async Task<List<InterviewReportViewModel>> GetInterviewWiseReport()
    {
        return await _reportRepository.GetInterviewWiseReport();
    }
}