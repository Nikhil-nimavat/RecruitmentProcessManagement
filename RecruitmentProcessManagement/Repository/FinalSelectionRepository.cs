﻿using System.Reflection.Metadata;
using DocumentFormat.OpenXml.Drawing;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class FinalSelectionRepository: Interfaces.IFinalSelectionRepository
    {
        private readonly Data.ApplicationDbContext _context;

        public FinalSelectionRepository(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate> GetCandidateById(int candidateId)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.CandidateID == candidateId);
        }

        // ERROR CODE: Library support needed

        public async Task<string> GenerateOfferLetter(int candidateId, int positionId, DateTime joiningDate)
        {
            string filePath = $"wwwroot/uploads/offer_letters/Offer_{candidateId}.pdf";

            //using (FileStream fs = new FileStream(filePath, FileMode.Create))
            //{
            //    Document doc = new Document();
            //    PdfWriter.GetInstance(doc, fs);
            //    doc.Open();
            //    doc.Add(new Paragraph($"Congratulations! You are selected for Position {positionId}. Your joining date is {joiningDate}."));
            //    doc.Close();
            //}

            return filePath;
        }

        public async Task MarkCandidateAsHired(int candidateId, string offerLetterPath)
        {
            var candidate = await _context.Candidates.FindAsync(candidateId);
            if (candidate != null)
            {
                candidate.ProfileStatus = "Hired";
                candidate.ModifiedDate = DateTime.Now;

                var finalSelection = new FinalSelection
                {
                    CandidateID = candidateId,
                    PositionID = candidate.FinalSelections.First().PositionID,
                    OfferLetterPath = offerLetterPath,
                    Status = "Selected",
                    JoiningDate = DateTime.Now.AddDays(15)
                };

                _context.FinalSelections.Add(finalSelection);
                await _context.SaveChangesAsync();
            }
        }
    }
}
