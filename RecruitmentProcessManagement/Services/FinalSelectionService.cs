using System.Reflection.Metadata;
using DocumentFormat.OpenXml.Drawing;
using iTextSharp.text.pdf;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class FinalSelectionService : IFinalSelectionService
    {
        public async Task<string> GenerateOfferLetter(int candidateId, int positionId, DateTime joiningDate)
        {
            string filePath = $"/uploads/offer_letters/Offer_{candidateId}.pdf";

            //Error in finding the right library to do the same

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
    }
}
