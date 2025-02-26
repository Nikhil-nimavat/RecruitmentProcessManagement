public interface IReportService
{
    Task<List<PositionReportViewModel>> GetPositionWiseReport();
    Task<List<CollegeReportViewModel>> GetCollegeWiseReport();
    Task<List<InterviewReportViewModel>> GetInterviewWiseReport();
}