using numberGet.Enums;

namespace numberGet.Models.GameModels
{
    public class GameModel
    {
        public int? UserGuess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int RemainingAttempts { get; set; }
        public string SelectedLevel { get; set; }
        public bool IsGameOver { get; set; } = false;
    }
}
