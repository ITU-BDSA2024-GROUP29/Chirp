namespace Chirp.Infrastructure.Chirp.Repository;
using Microsoft.EntityFrameworkCore;

public interface ICheepRepository {
    Task CreateCheepAsync(Cheep newCheep);
    Task<List<Cheep>> ReadCheeps();
    Task<List<Cheep>> GetPaginatedCheeps(int pageNumber, int pageSize);
    //Task UpdateCheep(Cheep alteredcheep);

    
}