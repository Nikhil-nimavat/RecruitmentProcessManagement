using System.Data;
using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class BulkHiringRepository : IBulkHiringRepository
    {
        private readonly ApplicationDbContext _context;
        public BulkHiringRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Candidate> GetCandidateByEmail(string email)
        {
            return await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email);
        }

        // email checking is not working fix it with debugging the code ==> Working properly break point yet to
        // find and test
        public async Task ImportCandidatesFromExcel(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                var email = row["email"];
                var result = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == email.ToString());

                if (result == null)
                {
                    var candidate = new Candidate
                    {
                        Name = row["Name"].ToString(),
                        Email = row["Email"].ToString(),
                        PhoneNumber = row["PhoneNumber"].ToString(),
                        CollegeName = row["CollegeName"].ToString(),
                        CreatedDate = DateTime.Now,
                        ProfileStatus = "Bulk Hire"
                    };

                    await _context.Candidates.AddAsync(candidate);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
