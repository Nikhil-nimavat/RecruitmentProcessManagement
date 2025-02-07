using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository;

namespace RecruitmentProcessManagement.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ICandidateRepository _repository;

        public CandidateController(ICandidateRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        { 
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Create(Candidate candidate)
        {
            if (ModelState.IsValid)
            { 
                await _repository.AddCandidate(candidate);
                return RedirectToAction("Index");
            }

            return View(candidate);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        { 
            var candidate = await _repository.GetCandidateById(id);
            if (candidate == null)
            {
                NotFound(); 
            }

            return View(candidate);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateCandidate(candidate);
                return RedirectToAction(nameof(Index));
            }
            return View(candidate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var candidate = await _repository.GetCandidateById(id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Candidate candidate)
        {
            await _repository.DeleteCandidateById(candidate.CandidateID);
            return View(Index);
        }
    }
}
