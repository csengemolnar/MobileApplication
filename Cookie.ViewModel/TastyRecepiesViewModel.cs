using Cookie.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookie.ViewModel
{
    public class TastyRecepiesViewModel:BindingSource
    {
        # region Fields

        private bool _isFavourite;

        #endregion
        public string? Name { get; private set; }
        public string? Yields { get; private set; } //Servings 
        public int? TotalTimeMinutes { get; private set; }
        public string? ThumbnailUrl { get; private set; }
        public List<int> Positions { get; private set; } //Instruction
        public List<string> DisplayTexts { get; private set; } //Instruction

        public List<string> Ingredients { get; private set; } //Ingredients - raw text

        public bool IsFavourite
        {
            get => _isFavourite;
            set
            {
                if (value != _isFavourite)
                {
                    _isFavourite = value;
                    OnPropertyChanged();
                }
            }
        }

        public DelegateCommand SelectCommand { get; private set; }
        public DelegateCommand InstructionCommand { get; private set; }
        public DelegateCommand FavouriteCommand { get; private set; }



        #region Constructor
        public TastyRecepiesViewModel(Model.DTO.TastySearchResult tastySearchResult, List<Model.DTO.TastyInstruction> tastydetailsList,
            List<Model.DTO.TastySection> sections, DelegateCommand delegateCommand, DelegateCommand delegateC, bool isFavourite, DelegateCommand favouriteCommand)
        {
            Name = tastySearchResult.name;
            Yields = tastySearchResult.yields;
            TotalTimeMinutes = tastySearchResult.total_time_minutes;
            ThumbnailUrl = tastySearchResult.thumbnail_url;
            IsFavourite = isFavourite;
            // Extract list to data members
            Positions = new List<int>();

            DisplayTexts = new List<string>();

            Ingredients = new List<string>();



            foreach(Model.DTO.TastyInstruction instruction in tastydetailsList)
            {
                Positions.Add(instruction.position);
                DisplayTexts.Add(instruction.display_text);
            }

            foreach(Model.DTO.TastySection section in sections)
            {
                foreach(Model.DTO.TastyComponent component in section.components)
                {
                    Ingredients.Add(component.raw_text);
                }
            }
            FavouriteCommand = favouriteCommand;
            SelectCommand = delegateCommand;
            InstructionCommand = delegateC;

        }

        public TastyRecepiesViewModel(Persistence.FavouriteRecepie fav, DelegateCommand selectCommand, DelegateCommand favouriteCommand)
        {
            Name = fav.Name;
            SelectCommand= selectCommand;
            FavouriteCommand = favouriteCommand;
            
        }
        #endregion

    }
}
