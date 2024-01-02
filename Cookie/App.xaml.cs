using Cookie.Model;
using Cookie.Persistence;
using Cookie.Persistence.TextJson;
using Cookie.ViewModel;

namespace Cookie;

public partial class App : Application
{
	
	private MainFlyoutPage _rootPage;
    private ICookiePersistence _persistence;
    private CookieModel _model;
	private CookieViewModel _viewModel;
    

    
    public App()
	{
		InitializeComponent();

        _persistence=new CookieTextJsonPersistence();

		_model = new CookieModel(_persistence);

		_viewModel = new CookieViewModel(_model);

        _viewModel.RecepieSelected += _viewModel_SelectedRecepie;
        _viewModel.RecepieInstruction += _viewModel_RecepieInstruction;

        _rootPage = new MainFlyoutPage();
        
        _rootPage.BindingContext = _viewModel;
		MainPage = _rootPage;
	}

    private async void _viewModel_RecepieInstruction(object sender, EventArgs e)
    {
        if (!(_rootPage.NavigationPage.CurrentPage is RecepieDetailsPage))
            await _rootPage.NavigationPage.PushAsync(new RecepieDetailsPage());
    }

    private async void _viewModel_RecepiesLoaded(object sender, EventArgs e)
    {
        if (!(_rootPage.NavigationPage.CurrentPage is SearchRecepiesPage))
            await _rootPage.NavigationPage.PushAsync(new SearchRecepiesPage());
    }

    private async void _viewModel_SelectedRecepie(object sender, EventArgs e)
    {
        if (!(_rootPage.NavigationPage.CurrentPage is IngredientsPage))
            await _rootPage.NavigationPage.PushAsync(new IngredientsPage());
    }

    protected override async void OnStart() => await _model.InitializeAsync();

}