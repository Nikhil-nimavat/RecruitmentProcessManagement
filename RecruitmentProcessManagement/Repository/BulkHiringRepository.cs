using System.Data;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class BulkHiringRepository : IBulkHiringRepository
    {
        private readonly Data.ApplicationDbContext _context;
        public BulkHiringRepository(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ImportCandidatesFromExcel(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                var candidate = new Candidate
                {
                    Name = row["Name"].ToString(),
                    Email = row["Email"].ToString(),
                    PhoneNumber = row["PhoneNumber"].ToString(),
                    CollegeName = row["CollegeName"].ToString(),
                    CreatedDate = DateTime.Now,
                    ProfileStatus = "New"
                };

                await _context.Candidates.AddAsync(candidate);
            }

            await _context.SaveChangesAsync();
        }
    }
}
