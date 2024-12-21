using Microsoft.AspNetCore.Mvc;
using music.Models;
using music.Services;
using music.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace music.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicalInstrumentsController : ControllerBase
    {
        private IMusicalInstruments MusicalInstrumentsService;
        public MusicalInstrumentsController(IMusicalInstruments MusicalInstrumentsService)
        {
            this.MusicalInstrumentsService = MusicalInstrumentsService;
        }

        [HttpGet]
        [Authorize(Policy = "User")]
        public ActionResult<List<MusicalInstruments>> GetAll() {
           return MusicalInstrumentsService.GetAll(int.Parse(User.FindFirst("id")?.Value!));
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "User")] 

        public ActionResult<MusicalInstruments> Get(int id)
        {
            var MI = MusicalInstrumentsService.Get(id);

            if (MI == null)
                return NotFound();

            return MI;
        }

        [HttpPost]
        [Authorize(Policy = "User")] 
        public IActionResult Create(MusicalInstruments MI)
        {
            MusicalInstrumentsService.Add(MI);
            return CreatedAtAction(nameof(Create), new {id=MI.Id}, MI);

        }

        [HttpPut("{id}")]
        [Authorize(Policy = "User")]
        public IActionResult Update(int id, MusicalInstruments MI)
        {
            if (id != MI.Id)
                return BadRequest();

            var existingMI = MusicalInstrumentsService.Get(id);
            if (existingMI is null)
                return  NotFound();

            MusicalInstrumentsService.Update(MI);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        public IActionResult Delete(int id)
        {
            var MI = MusicalInstrumentsService.Get(id);
            if (MI is null)
                return  NotFound();

            MusicalInstrumentsService.Delete(id);

            return Content(MusicalInstrumentsService.Count.ToString());
        }
    }
}