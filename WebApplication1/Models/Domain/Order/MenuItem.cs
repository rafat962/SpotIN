using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MenuItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Item name is required")]
    [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    // Foreign Key to WorkSpace
    public int WorkSpaceID { get; set; }
    [ForeignKey("WorkSpaceID")]
    public virtual WorkSpace WorkSpace { get; set; }
}