

namespace Chirp.Core.DomainModel ;

public class Author : ApplicationUser{
    public int AuthorId {get; set;}
    public String Name { get; set;}
    public String Email { get; set;}
    public ICollection<Cheep> Cheeps { get; set;}
    public ICollection<Author>? Follows { get; set;}

        public AuthorDTO toAuthorDTO(){
        return new AuthorDTO(
            this.AuthorId,
            this.Name,
            this.Email
        );
    }
    
}

public record AuthorDTO(int AuthorId, String Name, String Email);


