using Newtonsoft.Json;

namespace Cookie.Persistence.TextJson
{
    public class CookieTextJsonPersistence : ICookiePersistence
    {
        #region Properties

        private string _FavouritesFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favourites.json");

        #endregion

        #region Private Methods
        private async Task SaveFavouritesAsync(List<FavouriteRecepie> favourites)
        {
            try
            {
                string json = JsonConvert.SerializeObject(favourites);
                await File.WriteAllTextAsync(_FavouritesFilePath, json);
            }
            catch { }
        }
        private async Task<List<FavouriteRecepie>> LoadFavouritesAsync()
        {
            try
            {
                string filePath = _FavouritesFilePath;
                List<FavouriteRecepie>? favourites = null;
                if (File.Exists(filePath))
                {
                    string json = await File.ReadAllTextAsync(filePath);
                    favourites = JsonConvert.DeserializeObject<List<FavouriteRecepie>>(json);
                }
                return favourites ?? new List<FavouriteRecepie>();
            }
            catch
            {
                return new List<FavouriteRecepie>();
            }
        }
        #endregion

        #region Public Methods

        public async Task AddFavouriteAsync(FavouriteRecepie favourite)
        {
            List<FavouriteRecepie> favourites = await LoadFavouritesAsync();
            favourites.Add(favourite);
            await SaveFavouritesAsync(favourites);

        }

        public async Task<IEnumerable<FavouriteRecepie>> GetFavouriteRecepiesAsync() => await LoadFavouritesAsync();
        

        public async Task RemoveFavouriteAsync(string name)
        {
            List<FavouriteRecepie> favourites = await LoadFavouritesAsync();
            favourites.RemoveAll(f => f.Name == name);
            await SaveFavouritesAsync(favourites);
        }
        #endregion


    }
}