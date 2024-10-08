using Microsoft.EntityFrameworkCore;
namespace  Chirp.Razor.DomainModel ;

public class User {
    public int UserId {get; set;}
    public String Name { get; set;}
    public ICollection<Cheep> Cheeps { get; set;}
}

