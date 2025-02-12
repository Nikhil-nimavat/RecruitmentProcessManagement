using System.Reflection.Metadata;
using DocumentFormat.OpenXml.Drawing;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class FinalSelectionService : IFinalSelectionService
    {
        private readonly ApplicationDbContext _context;

        public FinalSelectionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate> GetCandidateById(int candidateId)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.CandidateID == candidateId);
        }

        // ERROR CODE: Need library to do following FileStream not woking properly

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
