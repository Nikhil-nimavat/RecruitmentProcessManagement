﻿using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface ICandidateService
    {
        Task AddCandidate(Candidate candidate);
    }
}
