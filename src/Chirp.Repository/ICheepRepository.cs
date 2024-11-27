using Chirp.Core.DomainModel;

namespace Chirp.Repository;


public interface ICheepRepository {
    Task CreateCheepAsync(Cheep newCheep);
    Task<List<Cheep>> ReadCheeps();
    Task<List<Cheep>> GetPaginatedCheeps(int pageNumber, int pageSize);
    //Task UpdateCheep(Cheep alteredcheep);
    Task<int> GetTotalCheepCount();
    Task<List<Cheep>> GetTotalCheepsFromAuthorCount(String authorname);
    Task<Author> GetAuthorByEmail(String Email);
    Task<int> GetTotalAuthorsCount();
    Task<Author> GetAuthorByName(String authorname);

    


    
}