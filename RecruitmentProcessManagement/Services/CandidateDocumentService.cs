using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class CandidateDocumentService : ICandidateDocumentService
    {
        private readonly ApplicationDbContext _context;
        public CandidateDocumentService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task AddDocument(CandidateDocument document)
        {
            await _context.CandidateDocuments.AddAsync(document);
            await _context.SaveChangesAsync();
        }
    }
}
