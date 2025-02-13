using System.Data;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public class BulkHiringService : IBulkHiringService
    {
        private readonly IBulkHiringRepository _repository;

        public BulkHiringService(IBulkHiringRepository repository)
        {
            _repository = repository;
        }
        public async Task ImportCandidatesFromExcel(DataTable dataTable)
        {
            await _repository.ImportCandidatesFromExcel(dataTable);
        }

        public async Task<Candidate> GetCandidateByEmail(string email)
        {
            return await _repository.GetCandidateByEmail(email);
        }
    }
}
