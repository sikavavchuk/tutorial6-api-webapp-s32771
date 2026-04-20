using WebApp.Models;

namespace WebApp.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(DataStore.Reservations);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var res = DataStore.Reservations.FirstOrDefault(r => r.Id == id);

        if (res == null)
            return NotFound();

        return Ok(res);
    }

    // FILTER
    [HttpGet]
    public IActionResult Filter(
        [FromQuery] DateTime? date,
        [FromQuery] string status,
        [FromQuery] int? roomId)
    {
        var query = DataStore.Reservations.AsQueryable();

        if (date.HasValue)
            query = query.Where(r => r.Date.Date == date.Value.Date);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status == status);

        if (roomId.HasValue)
            query = query.Where(r => r.RoomId == roomId);

        return Ok(query.ToList());
    }

    // POST
    [HttpPost]
    public IActionResult Create(Reservation reservation)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (reservation.EndTime <= reservation.StartTime)
            return BadRequest("EndTime must be later than StartTime");

        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);

        if (room == null)
            return NotFound("Room does not exist");

        if (!room.IsActive)
            return Conflict("Room is inactive");

        // overlap check
        bool overlap = DataStore.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date.Date == reservation.Date.Date &&
            reservation.StartTime < r.EndTime &&
            reservation.EndTime > r.StartTime
        );

        if (overlap)
            return Conflict("Reservation overlaps");

        reservation.Id = DataStore.Reservations.Max(r => r.Id) + 1;
        DataStore.Reservations.Add(reservation);

        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }

    // PUT
    [HttpPut("{id}")]
    public IActionResult Update(int id, Reservation updated)
    {
        var res = DataStore.Reservations.FirstOrDefault(r => r.Id == id);

        if (res == null)
            return NotFound();

        res.RoomId = updated.RoomId;
        res.OrganizerName = updated.OrganizerName;
        res.Topic = updated.Topic;
        res.Date = updated.Date;
        res.StartTime = updated.StartTime;
        res.EndTime = updated.EndTime;
        res.Status = updated.Status;

        return Ok(res);
    }

    // DELETE
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var res = DataStore.Reservations.FirstOrDefault(r => r.Id == id);

        if (res == null)
            return NotFound();

        DataStore.Reservations.Remove(res);

        return NoContent();
    }
}