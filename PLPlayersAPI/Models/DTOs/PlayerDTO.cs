namespace PLPlayersAPI.Models.DTOs
{
    public class PlayerDTO
    {
        public int PlayerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImgSrc { get; set; }
        public string? DateOfBirth { get; set; }
        public ClubDTO Club { get; set; }
        public NationalityDTO Nationality { get; set; }
        public PositionDTO Position { get; set; }
    }
}
