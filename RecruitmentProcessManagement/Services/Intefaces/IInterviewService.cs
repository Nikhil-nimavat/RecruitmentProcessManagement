using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface IInterviewService
    {
        Task<List<IdentityUser>> GetBestInterviewers(int positionId);

        Task<bool> AssignInterviewers(int candidateId, int positionId);

        Task SendMeetingInvites(int candidateId, List<string> interviewerIds, DateTime interviewDate);
    }
}
