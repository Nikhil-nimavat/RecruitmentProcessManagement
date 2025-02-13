using System.Data;
using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface IBulkHiringService
    {
        Task ImportCandidatesFromExcel(DataTable dataTable);

        Task<Candidate> GetCandidateByEmail(string email);
    }
}
