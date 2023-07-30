using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.Infrastructure.DataBaseContext;
using CitiesManager.Core.Entities;
using Microsoft.AspNetCore.Cors;

namespace CitiesManager.WebAPI.Controllers
{
    [EnableCors("4200Client")]
    public class CitiesController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Cities
        /// <summary>
        /// Returns all cities from cities table
        /// </summary>
        /// <returns>List of cities in json format</returns>
        [HttpGet]
        //[Produces("application/xml")]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
          if (_context.Cities is null)
              return NotFound();
          
          var cities = await _context.Cities.OrderBy(city => city.CityName).ToListAsync();
          return cities;
        }


        // GET: api/Cities/5
        /// <summary>
        /// Returns a specific city
        /// </summary>
        /// <param name="id"></param>
        /// <returns>City object in json format</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(Guid id)
        {
            if (_context.Cities == null)
                return Problem(detail: "DbContext does not exist", statusCode: 400, title: "DbContext");
            
            var city = await _context.Cities.FirstOrDefaultAsync(city => city.CityId == id);

            if(city is null)
            {
                return Problem(detail: "Id does not exist", instance: "City", statusCode: 404, title: "City Search");
            }

            // since we are returning an object as result, the task type should be ActionResult
            // if task type is IActionResult the return should be "Ok(city)"
            return city;
        }


        // PUT: api/Cities/5
        /// <summary>
        /// Updates a specific City status
        /// </summary>
        /// <param name="id">Guid</param>
        /// <param name="city">string</param>
        /// <returns>The city object updated in json format</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(Guid id, [Bind(nameof(City.CityId), nameof(City.CityName))] City city)
        {
            if (id != city.CityId)
                return BadRequest();
            
            // _context.Entry(city).State = EntityState.Modified; // update all properties of city object

            var cityObject = await _context.Cities.FindAsync(city);

            if (cityObject is null)
                return NotFound();

            cityObject.CityName = city.CityName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Cities
        /// <summary>
        /// Inserts a new city object to the Cities table
        /// </summary>
        /// <param name="city">City object trough model binding</param>
        /// <returns>The inserted city in json format</returns>
        [HttpPost]
        public async Task<ActionResult<City>> PostCity([Bind(nameof(City.CityId), nameof(City.CityName))] City city)
        {
          if (_context.Cities == null)
              return Problem("Entity set 'ApplicationDbContext.Cities'  is null.");

          /*if (!ModelState.IsValid)
              return ValidationProblem(ModelState);*/
          
          _context.Cities.Add(city);
          await _context.SaveChangesAsync();

          return CreatedAtAction("GetCity", new { id = city.CityId }, city);
        }


        // DELETE: api/Cities/5
        /// <summary>
        /// Removes a specific city form Cities table
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            if (_context.Cities is null)
                return NotFound();
            
            var city = await _context.Cities.FindAsync(id);

            if (city is null)
                return NotFound();
            
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(Guid id) => (_context.Cities?.Any(e => e.CityId == id)).GetValueOrDefault();
        
    }
}
