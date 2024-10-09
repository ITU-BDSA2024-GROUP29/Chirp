using Chirp.Razor.DomainModel;

namespace Chirp.Razor.CheepRepository;

public interface ICheepRepository {
    Task CreateCheep(Cheep newCheep);
    Task<List<Cheep>> ReadCheeps(String userName);
    Task UpdateCheep(Cheep alteredcheep);
}