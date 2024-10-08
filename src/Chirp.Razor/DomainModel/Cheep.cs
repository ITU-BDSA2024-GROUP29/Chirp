using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace  Chirp.Razor.DomainModel;
    




public class Cheep { 
    [Key]
    public int MessageId { get; set; }
    
    public int UserId { get; set; }
    public String Text { get; set; }
    public User User { get; set; }

}
