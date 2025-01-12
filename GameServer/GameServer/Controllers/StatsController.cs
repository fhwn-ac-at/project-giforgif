using GameServer.Data;
using GameServer.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace GameServer.Controllers
{
    [ApiController]
    [Route("stats")]
    public class StatsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public StatsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Player>>> GetTopPlayers()
        {
            var topPlayers = await _context.Players
                .OrderByDescending(p => p.Won)
                .Take(20)
                .ToListAsync();

            return Ok(topPlayers);
        }
    }
}
