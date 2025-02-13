using System.Data;
using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface IBulkHiringRepository
    {
        Task ImportCandidatesFromExcel(DataTable dataTable);
        Task<Candidate> GetCandidateByEmail(string email);
    }
}
