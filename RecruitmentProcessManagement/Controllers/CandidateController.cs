using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    [Authorize(Roles = "Admin, Reviewer")]
    public class CandidateController : Controller
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        public async Task<IActionResult> Index()
        {
            var candidates = await _candidateService.GetAllCandidatesAsync();
            return View(candidates);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Candidate candidate)
        {
            if (ModelState.IsValid)
            { 
                await _candidateService.AddCandidate(candidate);
                return RedirectToAction("Index");
            }

            return View(candidate);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var candidate = await _candidateService.GetCandidateById(id);
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
                await _candidateService.UpdateCandidate(candidate);
                return RedirectToAction("Index");
            }
            return View(candidate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var candidate = await _candidateService.GetCandidateById(id);
            if (candidate == null)
            {
                return NotFound();
            }
            return View(candidate);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Candidate candidate)
        {
            await _candidateService.DeleteCandidateById(candidate.CandidateID);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> SkillList()
        {
            var skills = await _candidateService.GetSkillList();
            return View(skills);
        }

        [HttpGet]
        public IActionResult CreateSkill()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkill(Skill skill)
        {
            if (ModelState.IsValid)
            {
                await _candidateService.AddSkill(skill);
                return RedirectToAction("SkillList");
            }
            return View(skill);
        }

        [HttpGet]
        public async Task<IActionResult> EditSkill(int id)
        {
            var skill = await _candidateService.GetSkillById(id);
            if (skill == null)
            {
                NotFound();
            }

            return View(skill);

        }

        [HttpPost]
        public async Task<IActionResult> EditSkill(Skill skill)
        {
            if (ModelState.IsValid)
            {
                await _candidateService.UpdateSkill(skill);
                return RedirectToAction("SkillList");
            }
            return View(skill);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var skill = await _candidateService.GetSkillById(id);
            if (skill == null) return NotFound();
            return View(skill);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSkill(Skill skill)
        {
            await _candidateService.DeleteSkill(skill.SkillID);
            return View("SkillList");
        }
    }
}