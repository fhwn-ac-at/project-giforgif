using GameServer.Models;
using GameServer.Stores;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateRoom(RoomRequest request)
        {
            if (RoomStore.Exists(request.RoomName))
            {
                return BadRequest(new { Message = $"Room '{request.RoomName}' already exists." });
            }

            if (RoomStore.Add(request.RoomName))
            {
                return Ok(new { Message = $"Room '{request.RoomName}' created." });
            }
            
            return StatusCode(500, new { Message = "An error occurred while creating the room." });
        }

        [HttpGet]
        public IActionResult GetRooms()
        {
            var rooms = RoomStore.GetAllRoomNames();

            List<RoomResponse> responses = new List<RoomResponse>();

            foreach (var room in rooms)
            {
                Game game = RoomStore.GetGame(room);
                responses.Add(new RoomResponse(room, game.Started, game.Players.Count));
            }

            return Ok(responses);
        }
    }
}
