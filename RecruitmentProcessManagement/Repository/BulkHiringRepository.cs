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

        // email checking is not working fix it with debugging the code ==> Now Working properly break point yet to
        // find and test
        // test case - 1
        // test on 1000 row will take a lot of  time
        // test case - 2
        // test on invlid data 
        // test case - 3
        //
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
                        ResumePath = row["ResumePath"].ToString(),
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
