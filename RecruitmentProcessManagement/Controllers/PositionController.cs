using Microsoft.AspNetCore.Mvc;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Services;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var positions = await _positionService.GetAllPositionsAsync();
            return View(positions);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (ModelState.IsValid)
            {
                await _positionService.AddPositionAsync(position);
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var position = await _positionService.GetPositionByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Position position)
        {
            if (id != position.PositionID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _positionService.UpdatePositionAsync(position);
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var position = await _positionService.GetPositionByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _positionService.DeletePositionAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Close(int id, string ReasonForClosure)
        {
            var position = await _positionService.GetPositionByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }

            position.Status = "Closed";
            position.ReasonForClosure = ReasonForClosure;
            await _positionService.UpdatePositionAsync(position);

            return RedirectToAction(nameof(Index));
        }
    }
}

