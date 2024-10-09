using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace  Chirp.Razor.DomainModel;
    




public class Cheep { 
    [Key]
    public int CheepId { get; set; }
    
    public int AuthorId { get; set; }
    public String Text { get; set; }
    public Author Author { get; set; }
    public int TimeStamp { get; set; }

}
