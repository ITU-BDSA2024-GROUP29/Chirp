using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace  Chirp.Razor.DomainModel;
    




public class Cheep { 
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public String Text { get; set; }
    public Author Author { get; set; }
    public System.DateTime TimeStamp { get; set; }

}
