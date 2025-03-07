﻿using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface ICandidateDocumentService
    {
        Task AddDocument(CandidateDocument document);
        Task<Candidate> GetCandidateById(int id);
    }
}
