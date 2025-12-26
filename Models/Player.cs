namespace BasketballApi.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int Height { get; set; }
        public string Team { get; set; } = string.Empty;
        public double PointsPerGame { get; set; }

        // Конструктор по умолчанию (обязателен для .NET 8/10)
        public Player() { }

        // Конструктор с параметрами
        public Player(int id, string name, string position, int height, string team, double pointsPerGame)
        {
            Id = id;
            Name = name;
            Position = position;
            Height = height;
            Team = team;
            PointsPerGame = pointsPerGame;
        }
    }
}