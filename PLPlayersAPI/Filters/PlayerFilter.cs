namespace PLPlayersAPI.Filters
{
    public class PlayerFilter
    {
        public string? Country { get; set; } = null;
        public string? Club { get; set; } = null;
        public string? Position { get; set; } = null;

        public PlayerFilter() { }
    }
}
