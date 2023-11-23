using System.Linq;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.AspNetCore;
using anime_climax_api.Database;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace anime_climax_api.Models;

public class Query {
        // private readonly DataContext _db;

        // public Query([Service] DataContext db)
        // {
        //     _db = db;
        // }

        // [UseSorting]
        // [UseFiltering]
        [UseFiltering]
        public IQueryable<Animes> GetAnimes(DataContext db) => db.Animes;

        [UseOffsetPaging(IncludeTotalCount = true, MaxPageSize = 50)]
        [UseFiltering]
        public IQueryable<Clips> GetClips(DataContext db) => db.Clips;
    
}

public class Mutation {

        async public Task<Animes> AddNewAnime(DataContext db, String secret, Animes anime) {
                try {
                        if (!secret.Equals("pichan321")) {
                                throw new Exception("Unauthorized");
                        }
           
                        Animes? last = await db.Animes.OrderByDescending(anime => anime.ID).FirstOrDefaultAsync();
                        if (last is null) {throw new Exception("Internal server error");}
                        anime.ID = last.ID + 1;


                        await db.AddAsync(anime);
                        await db.SaveChangesAsync();
                        return anime;
                } catch {
                        throw;
                }
        }
}

public class Subscription {

}
