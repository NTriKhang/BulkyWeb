using System.ComponentModel.DataAnnotations;

namespace Bulky.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DisplayOrder { set; get; }
    }
}
