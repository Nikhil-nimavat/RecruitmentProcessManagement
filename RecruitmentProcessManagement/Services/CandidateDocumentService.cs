using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository;
using RecruitmentProcessManagement.Repository.Interfaces;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class CandidateDocumentService : ICandidateDocumentService
    {
        private readonly ICandidateDocumentRepository _repository;
        public CandidateDocumentService(ICandidateDocumentRepository repository) 
        {
            _repository = repository;
        }

        public async Task AddDocument(CandidateDocument document)
        {
            await _repository.AddDocument(document);
        }
        public async Task<Candidate> GetCandidateById(int id)
        {
            return await _repository.GetCandidateById(id);
        }

    }
}
