using System.Reflection.Metadata;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class FinalSelectionRepository: Interfaces.IFinalSelectionRepository
    {
        private readonly ApplicationDbContext _context;

        public FinalSelectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate> GetCandidateById(int candidateId)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.CandidateID == candidateId);
        }

        public async Task<string> GenerateOfferLetter(int candidateId, int positionId, DateTime joiningDate)
        {
            string filepath = $"wwwroot/uploads/offer_letters/Offer_{candidateId}.pdf";
            string directorypath = Path.GetDirectoryName(filepath);    

            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }

            using (FileStream fs = new FileStream(filepath, FileMode.Create))
            {
                iTextSharp.text.Document doc = new iTextSharp.text.Document();
                PdfWriter.GetInstance(doc, fs);
                doc.Open();
                
                iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph($"Congratulations! You are selected for Position {positionId}. Your joining date is {joiningDate}.");
                doc.Add(paragraph);

                doc.Close();
            }

            return filepath;
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
