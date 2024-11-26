
namespace Chirp.Core.DomainModel ;

public class Author : ApplicationUser{
    public int AuthorId {get; set;}
    public String Name { get; set;}
    public String Email { get; set;}
    public ICollection<Cheep> Cheeps { get; set;}
    public ICollection<ApplicationUser> Follows { get; set;}

    public static implicit operator Author?(Task<Author>? v)
    {
        throw new NotImplementedException();
    }
}

