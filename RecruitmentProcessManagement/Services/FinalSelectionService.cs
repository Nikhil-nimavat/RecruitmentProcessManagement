using System.Reflection.Metadata;
using DocumentFormat.OpenXml.Drawing;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Services
{
    public class FinalSelectionService : IFinalSelectionService
    {
        private readonly IFinalSelectionRepository _repository;

        public FinalSelectionService(IFinalSelectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<Candidate> GetCandidateById(int candidateId)
        {
            return await _repository.GetCandidateById(candidateId);
        }

        public async Task<string> GenerateOfferLetter(int candidateId, int positionId, DateTime joiningDate)
        {
            return await _repository.GenerateOfferLetter(candidateId, positionId, joiningDate);   
        }

        public async Task MarkCandidateAsHired(int candidateId, string offerLetterPath)
        {
            await _repository.MarkCandidateAsHired(candidateId, offerLetterPath);
            
        }
    }
}
