using DocumentFormat.OpenXml.Spreadsheet;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface IInterviewService
    {
        //Task<List<Users>> GetBestInterviewers(int positionId);

        Task<bool> AssignInterviewers(string candidateId, int positionId);

        Task SendMeetingInvites(string candidateId, List<string> interviewerIds, DateTime interviewDate);
    }
}
