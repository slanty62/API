using BasketballApi.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BasketballApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private static List<Player> _players = new()
        {
            new Player { Id = 10, Name = "Tetsuya Kuroko", Position = "PG", Height = 168, Team = "Seirin High", PointsPerGame = 5.8 },
            new Player { Id = 11, Name = "Taiga Kagami", Position = "PF", Height = 190, Team = "Seirin High", PointsPerGame = 22.5 },
            new Player { Id = 4, Name = "Ryota Kise", Position = "SG", Height = 189, Team = "Kaijo High", PointsPerGame = 26.4 },
            new Player { Id = 5, Name = "Shintaro Midorima", Position = "SG", Height = 195, Team = "Shutoku High", PointsPerGame = 29.2 },
            new Player { Id = 6, Name = "Atsushi Murasakibara", Position = "C", Height = 208, Team = "Yosen High", PointsPerGame = 28.1 },
            new Player { Id = 7, Name = "Daiki Aomine", Position = "SF", Height = 192, Team = "Tōō Academy", PointsPerGame = 31.5 },
            new Player { Id = 9, Name = "Seijuro Akashi", Position = "PG", Height = 173, Team = "Rakuzan High", PointsPerGame = 25.8 }
        };

        /// <summary>
        /// Получить всех баскетболистов
        /// </summary>
        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all players", Description = "Returns all players with optional sorting")]
        public IActionResult GetAll([FromQuery] int? sortStrategy = null)
        {
            try
            {
                var players = new List<Player>(_players);

                switch (sortStrategy)
                {
                    case null:
                        break;
                    case 1:
                        players = players.OrderBy(p => p.PointsPerGame).ToList();
                        break;
                    case -1:
                        players = players.OrderByDescending(p => p.PointsPerGame).ToList();
                        break;
                    default:
                        return BadRequest(new { error = "Некорректное значение параметра sortStrategy" });
                }

                return Ok(players);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Ошибка: {ex.Message}" });
            }
        }

        /// <summary>
        /// Получить игрока по номеру
        /// </summary>
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Get player by number")]
        public IActionResult GetPlayerById(int id)
        {
            try
            {
                if (id < 0)
                    return BadRequest(new { error = "Номер не может быть отрицательным" });

                var player = _players.FirstOrDefault(p => p.Id == id);

                return player == null
                    ? NotFound(new { error = $"Игрок №{id} не найден" })
                    : Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Ошибка: {ex.Message}" });
            }
        }

        /// <summary>
        /// Получить количество игроков в команде
        /// </summary>
        [HttpGet("team/count")]
        [SwaggerOperation(Summary = "Count players by team")]
        public IActionResult GetPlayersCountByTeam([FromQuery] string teamName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(teamName))
                    return BadRequest(new { error = "Укажите название команды" });

                var count = _players.Count(p =>
                    p.Team.Equals(teamName, StringComparison.OrdinalIgnoreCase));

                return Ok(new
                {
                    team = teamName,
                    playersCount = count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Ошибка: {ex.Message}" });
            }
        }

        /// <summary>
        /// Добавить нового игрока
        /// </summary>
        [HttpPost("add")]
        [SwaggerOperation(Summary = "Add new player")]
        public IActionResult AddPlayer([FromBody] Player newPlayer)
        {
            try
            {
                if (newPlayer == null)
                    return BadRequest(new { error = "Данные игрока обязательны" });

                if (newPlayer.Id <= 0)
                    return BadRequest(new { error = "Номер должен быть положительным" });

                if (_players.Any(p => p.Id == newPlayer.Id))
                    return Conflict(new { error = $"Игрок №{newPlayer.Id} уже существует" });

                _players.Add(newPlayer);

                return CreatedAtAction(nameof(GetPlayerById),
                    new { id = newPlayer.Id },
                    new { message = "Игрок добавлен!", player = newPlayer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Ошибка: {ex.Message}" });
            }
        }

        /// <summary>
        /// Удалить игрока
        /// </summary>
        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Delete player")]
        public IActionResult DeletePlayer(int id)
        {
            try
            {
                if (id < 0)
                    return BadRequest(new { error = "Номер не может быть отрицательным" });

                var player = _players.FirstOrDefault(p => p.Id == id);

                if (player == null)
                    return NotFound(new { error = $"Игрок №{id} не найден" });

                _players.Remove(player);

                return Ok(new
                {
                    message = $"Игрок {player.Name} удален",
                    deletedPlayer = player
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Ошибка: {ex.Message}" });
            }
        }
    }
}