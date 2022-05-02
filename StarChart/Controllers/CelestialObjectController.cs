using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
            return Ok(celestialObject);
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name);
            if (!celestialObjects.Any())
            {
                return NotFound();
            }
            foreach (var celestialObject in celestialObjects)
            {

                celestialObject.Satellites = _context.CelestialObjects
                    .Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects.ToList());
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach(var obj in celestialObjects)
            {
                obj.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == obj.Id).ToList();
            }
            return Ok(celestialObjects);
        }
    }
}
