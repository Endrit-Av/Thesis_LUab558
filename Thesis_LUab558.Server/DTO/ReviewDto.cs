namespace Thesis_LUab558.Server.DTO
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Rating { get; set; }

        public string? ReviewText { get; set; }

        public string? ReviewDate { get; set; }

        public string? UserName { get; set; }
    }
}
