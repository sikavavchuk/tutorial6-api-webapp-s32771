using WebApp.Models;

namespace WebApp.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    // GET all
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(DataStore.Rooms);
    }

    // GET by id
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);

        if (room == null)
            return NotFound();

        return Ok(room);
    }

    // GET by building (route param)
    [HttpGet("building/{buildingCode}")]
    public IActionResult GetByBuilding(string buildingCode)
    {
        var rooms = DataStore.Rooms
            .Where(r => r.BuildingCode == buildingCode)
            .ToList();

        return Ok(rooms);
    }

    // GET with query filters
    [HttpGet]
    public IActionResult Filter(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly)
    {
        var query = DataStore.Rooms.AsQueryable();

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity);

        if (hasProjector.HasValue)
            query = query.Where(r => r.HasProjector == hasProjector);

        if (activeOnly == true)
            query = query.Where(r => r.IsActive);

        return Ok(query.ToList());
    }

    // POST
    [HttpPost]
    public IActionResult Create(Room room)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        room.Id = DataStore.Rooms.Max(r => r.Id) + 1;
        DataStore.Rooms.Add(room);

        return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    // PUT
    [HttpPut("{id}")]
    public IActionResult Update(int id, Room updated)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);

        if (room == null)
            return NotFound();

        room.Name = updated.Name;
        room.BuildingCode = updated.BuildingCode;
        room.Floor = updated.Floor;
        room.Capacity = updated.Capacity;
        room.HasProjector = updated.HasProjector;
        room.IsActive = updated.IsActive;

        return Ok(room);
    }

    // DELETE
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);

        if (room == null)
            return NotFound();
        
        if (DataStore.Reservations.Any(r => r.RoomId == id))
            return Conflict("Room has reservations");

        DataStore.Rooms.Remove(room);

        return NoContent();
    }
}