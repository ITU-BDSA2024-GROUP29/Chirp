using Chirp.Razor.DomainModel;

namespace Chirp.Razor.CheepRepository;

public interface ICheepRepository {
    Task CreateCheepAsync(Cheep newCheep);
    Task<List<Cheep>> ReadCheeps();
    Task<List<Cheep>> GetPaginatedCheeps(int pageNumber, int pageSize);
    //Task UpdateCheep(Cheep alteredcheep);
    Task<List<Cheep>> GetTotalCheepsFromAuthorCount(String authorname);

    
}