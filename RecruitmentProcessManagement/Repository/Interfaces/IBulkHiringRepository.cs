using System.Data;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface IBulkHiringRepository
    {
        Task ImportCandidatesFromExcel(DataTable dataTable);
    }
}
