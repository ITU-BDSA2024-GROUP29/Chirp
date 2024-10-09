using Microsoft.EntityFrameworkCore;
namespace  Chirp.Razor.DomainModel ;

public class Author {
    public int AuthorId {get; set;}
    public String Name { get; set;}
    public String Email { get; set;}
    public ICollection<Cheep> Cheeps { get; set;}
}

