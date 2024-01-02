using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookie.Persistence
{
    public interface ICookiePersistence
    {
        Task AddFavouriteAsync(FavouriteRecepie favourite);
        Task RemoveFavouriteAsync(string name);
        Task<IEnumerable<FavouriteRecepie>> GetFavouriteRecepiesAsync();
    }
}
