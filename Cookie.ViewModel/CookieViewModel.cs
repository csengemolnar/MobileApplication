using Cookie.Model.DTO;
using Cookie.Persistence;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace Cookie.ViewModel
{
    public class CookieViewModel : BindingSource
    {
        #region Fields

        private Model.CookieModel _model;

        
        private TastyRecepiesViewModel _tastyRecepie;
        


        #endregion
        #region Events

        public event EventHandler? RecepiesLoaded;
        public event EventHandler? RecepieSelected;
        //Next to the instruction page
        public event EventHandler? RecepieInstruction;

        #endregion
        #region Properties

        public string SearchFilter { get; set; }
        public string Name { get;  set; }
        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand InstructionCommand { get; private set; }

      




        public ObservableCollection<TastyRecepiesViewModel> TastyRecepies { get; private set; } = new ObservableCollection<TastyRecepiesViewModel>();

    
        public TastyRecepiesViewModel TastyRecepie
        {
            get => _tastyRecepie;
            private set
            {
                if (value != _tastyRecepie)
                {
                    _tastyRecepie = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<TastyRecepiesViewModel> Favourites { get; private set; } = new ObservableCollection<TastyRecepiesViewModel>();


        #endregion

        #region Constructors
        public CookieViewModel(Model.CookieModel model)
        {
            _model = model ?? throw new ArgumentNullException("model");

            _model.TastySearchResultsLoaded += _model_TastySearchResultsLoaded;
            
            _model.FavouritesChanged += _model_FavouriteChanged;

            SearchCommand = new DelegateCommand(Command_Search);

           




        }


        private void _model_FavouriteChanged(object? sender, EventArgs e)
        {
            Favourites.Clear();
            foreach (Persistence.FavouriteRecepie fav in _model.Favourites)
                Favourites.Add(new TastyRecepiesViewModel(fav, new DelegateCommand(Command_FavouriteSelect), new DelegateCommand(Command_FavouriteRecepie)));

            foreach (TastyRecepiesViewModel recepie in TastyRecepies)
                recepie.IsFavourite = _model.Favourites.Any(f => f.Name == recepie.Name);
            if (_tastyRecepie != null)
                _tastyRecepie.IsFavourite = _model.Favourites.Any(f => f.Name == _tastyRecepie.Name);
        }



        private void _model_TastySearchResultsLoaded(object? sender, EventArgs e)
        {
            if (_model.TastySearchResults != null)
            {
                foreach (Model.DTO.TastySearchResult searchResult in _model.TastySearchResults)
                    TastyRecepies.Add(new TastyRecepiesViewModel(searchResult, searchResult.instructions, searchResult.sections, new DelegateCommand(Command_SelectRecepie), new DelegateCommand(Command_Instruction),
                        _model.Favourites.Any(f => f.Name == searchResult.name), 
                        new DelegateCommand(Command_FavouriteRecepie)));
            }
        }
        #endregion

        #region Command Methods

        private async void Command_FavouriteSelect(object param)
        {

            if (param != null && param is TastyRecepiesViewModel tastyrecepie)
            {
                try
                {
                    TastyRecepies.Clear();
                    await _model.SearchRecepiesAsync(tastyrecepie.Name.Trim());

                    if (TastyRecepies.Any())
                    {
                        foreach (TastyRecepiesViewModel item in TastyRecepies)
                        {
                            if (item.Name.Equals(tastyrecepie.Name))
                            {
                                TastyRecepie = item;
                                OnRecepieSelected();

                            }
                            
                        }
                        
                    }

                }
                catch (Exception ex)
                {

                    Debug.WriteLine($"An error occurred: {ex.Message}");
                }
               

            }



        }

        private async void Command_FavouriteRecepie(object param)
        {
            if (param != null && param is TastyRecepiesViewModel recepie)
            {
                if (recepie.IsFavourite)
                    await _model.RemoveFavouriteAsync(recepie.Name);

                else await _model.AddFavouriteAsync(recepie.Name);
            }
        }
        private async void Command_Search(object? param)
        {
            TastyRecepies.Clear();
            await _model.SearchRecepiesAsync(SearchFilter);
        }
        //FONTOS!! Nem a részletes leírásnak a viewmodel-jét kell megadni!!
        private void Command_SelectRecepie(object? param)
        {
            if(param != null && param is TastyRecepiesViewModel tastyrecepie)
            {
                TastyRecepie = tastyrecepie;
                OnRecepieSelected();
                

            }
        }

        private void Command_Instruction(object? param)
        {
            OnTastyInstructionsSelected();
        }
        #endregion

        #region Private Methods
        private void OnRecepieSelected() => RecepieSelected?.Invoke(this, EventArgs.Empty);
        private void OnTastyInstructionsSelected() => RecepieInstruction?.Invoke(this, EventArgs.Empty);

       

        #endregion
    }
}