using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CretaceousApi.Models;

namespace CretaceousApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AnimalsController : ControllerBase
  {
    private readonly CretaceousApiContext _db;

    public AnimalsController(CretaceousApiContext db)
    {
      _db = db;
    }

    // GET api/animals
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Animal>>> Get(string species, string name, int minimumAge, int age)
    {
      IQueryable<Animal> query = _db.Animals.AsQueryable();
      if (species != null)
      {
        query = query.Where(e => e.Species == species);
      }
      if (name != null)
      {
        query = query.Where(e => e.Name == name);
      }
      if (minimumAge > 0)
      {
        query = query.Where(e => e.Age >= minimumAge);
      }
      if (age != 0)
      {
        query = query.Where(e => e.Age == age);
      }
      return await query.ToListAsync();
    }

    // GET: api/Animals/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Animal>> GetAnimal(int id)
    {
      Animal animal = await _db.Animals.FindAsync(id);
      if (animal == null)
      {
        return NotFound();
      }
      return animal;
    }

    [HttpPost]
    public async Task<ActionResult<Animal>> Post(Animal animal)
    {
      _db.Animals.Add(animal);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetAnimal), new { id = animal.AnimalId }, animal);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Animal animal)
    {
      if (id != animal.AnimalId)
      {
        return BadRequest();
      }
      _db.Animals.Update(animal);
      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!AnimalExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }
      return NoContent();
      // http status code of 204 No Content: the request has completed successfully and no need to navigate away from the current page 
    }
    private bool AnimalExists(int id)
    {
      return _db.Animals.Any(e => e.AnimalId == id);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
      Animal animal = await _db.Animals.FindAsync(id);
      if (animal == null)
      {
        return NotFound();
      }
      _db.Animals.Remove(animal);
      await _db.SaveChangesAsync();
      return NoContent();
    }
  }
}