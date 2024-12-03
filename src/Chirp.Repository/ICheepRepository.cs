using Chirp.Core.DomainModel;

namespace Chirp.Repository;


public interface ICheepRepository {
    Task CreateCheepAsync(Cheep newCheep);
    Task<List<Cheep>> ReadCheeps();
    Task<List<Cheep>> GetPaginatedCheeps(int pageNumber, int pageSize);
    //Task UpdateCheep(Cheep alteredcheep);
    Task<int> GetTotalCheepCount();
    Task FollowAuthor(string authorname, string loggedinauthorname);
    Task<List<Cheep>> GetTotalCheepsFromAuthorCount(String authorname);
    Task<Author> GetAuthorByName(String authorname);
    Task<int> GetTotalAuthorsCount();
    Task<List<Author>> GetAuthors();
    Task<List<Author>> GetFollowedByAuthor(String authorname);
    Task AddFollowed(Author user, Author loggedinUser);
    Task<bool> DeleteCheepByID(int cheepID);





}