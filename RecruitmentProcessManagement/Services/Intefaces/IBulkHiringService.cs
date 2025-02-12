using System.Data;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface IBulkHiringService
    {
        Task ImportCandidatesFromExcel(DataTable dataTable);
    }
}
