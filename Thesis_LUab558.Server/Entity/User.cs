using System.ComponentModel.DataAnnotations.Schema;

namespace Thesis_LUab558.Server.Entity
{
    [Table("users")]
    public class User
    {
        [Column("user_id")]
        public int UserId { get; set; } // Primärschlüssel

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("street")]
        public string Street { get; set; }

        [Column("house_number")]
        public string HouseNumber { get; set; }

        [Column("city")]
        public string City { get; set; }

        [Column("postcode")]
        public int Postcode { get; set; }

        [Column("date_of_birth")]
        public string DateOfBirth { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("telephone")]
        public string Telephone { get; set; }

        [Column("is_employee")]
        public bool IsEmployee { get; set; }

        [Column("account_created_at")]
        public DateTime AccountCreatedAt { get; set; }
    }
}
