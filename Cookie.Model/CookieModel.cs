using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Cookie.Model
{
    public class CookieModel
    {
        #region Fields

        private Persistence.ICookiePersistence _persistence;

        private List<Persistence.FavouriteRecepie> _favourites = new List<Persistence.FavouriteRecepie>();

        #endregion

        #region Properties
        public IEnumerable<DTO.TastySearchResult>? TastySearchResults { get; private set; }
        
        public IEnumerable<Persistence.FavouriteRecepie> Favourites => _favourites;
        #endregion

        #region Events
        public event EventHandler? TastySearchResultsLoaded;
        public event EventHandler? TastyRecepieSelected;
        public event EventHandler? FavouritesChanged;
        

        #endregion

        #region Constructor
        public CookieModel(Persistence.ICookiePersistence persistence)
        {
            _persistence = persistence;
            
        }
        #endregion

        #region Public Methods

        public async Task InitializeAsync()
        {
            if (_persistence != null)
            {
                _favourites = (await _persistence.GetFavouriteRecepiesAsync()).ToList();
                FavouritesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task SearchRecepiesAsync(string word)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                Uri uri = new Uri($"https://tasty.p.rapidapi.com/recipes/list?from=0&size=10&q={word}");

                using (HttpClient client=new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "d824c9f0a9msh2fa6e523e857f35p1dcca8jsn73567567dcc4");
                    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "tasty.p.rapidapi.com");
                    using HttpResponseMessage response = await client.GetAsync(uri);
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                    if (response.IsSuccessStatusCode)
                    {
                        DTO.TastyResponse? tastyResponse=JsonConvert.DeserializeObject<DTO.TastyResponse>(await response.Content.ReadAsStringAsync());
                        if (tastyResponse != null && tastyResponse.Results != null)
                        {
                            TastySearchResults=tastyResponse.Results;
                            TastySearchResultsLoaded?.Invoke(this, EventArgs.Empty);

                        }
                    }
                }




            }

        }


        

        public async Task AddFavouriteAsync(string name)
        {
            if (_favourites.All(f => f.Name != name))
            {
                Persistence.FavouriteRecepie favourite = new Persistence.FavouriteRecepie() {Name = name };
                _favourites.Add(favourite);
                FavouritesChanged?.Invoke(this, EventArgs.Empty);

                if (_persistence != null)
                    await _persistence.AddFavouriteAsync(favourite);
            }
        }
        public async Task RemoveFavouriteAsync(string name)
        {
            Persistence.FavouriteRecepie? favourite = _favourites.FirstOrDefault(f => f.Name == name);
            if (favourite != null)
            {
                _favourites.Remove(favourite);
                FavouritesChanged?.Invoke(this, EventArgs.Empty);

                if (_persistence != null)
                    await _persistence.RemoveFavouriteAsync(name);
            }
        }

        #endregion
    }
}