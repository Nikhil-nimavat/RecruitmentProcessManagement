using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class CandidateDocumentRepository: ICandidateDocumentRepository
    {
        private readonly Data.ApplicationDbContext _context;

        public CandidateDocumentRepository(Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddDocument(CandidateDocument document)
        {
            await _context.CandidateDocuments.AddAsync(document);
            await _context.SaveChangesAsync();
        }
        public async Task<Candidate> GetCandidateById(int id)
        {
            var candidate = await _context.Candidates.FindAsync(id);
            return candidate;
        }
    }
}
