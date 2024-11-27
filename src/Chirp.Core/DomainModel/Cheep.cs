namespace  Chirp.Core.DomainModel;
    




public class Cheep { 
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public String Text { get; set; }
    public Author Author { get; set; }
    public System.DateTime TimeStamp { get; set; }

    public CheepDTO toCheepDTO(){
        return new CheepDTO(
            this.CheepId,
            this.Text,
            this.Author.Name,
            this.TimeStamp.ToString()
        );
    }

}

public record CheepDTO(int CheepId, String Text, String AuthorName, String TimeStamp);
