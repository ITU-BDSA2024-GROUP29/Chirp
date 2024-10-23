using Chirp.Razor.DomainModel;

namespace Chirp.Razor.CheepRepository;

public interface ICheepRepository {
    Task CreateCheepAsync(Cheep newCheep);
    Task<List<Cheep>> ReadCheeps(String userName);
    
    Task<List<Cheep>> GetPaginatedCheeps(int pageNumber, int pageSize);
    Task UpdateCheep(Cheep alteredcheep);

    
}