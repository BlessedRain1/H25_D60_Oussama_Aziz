using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using PartageDepense.Model;

namespace PartageDepense.ViewModel
{
    public partial class GestionnaireVM : ObservableObject
    {
        public Gestionnaire _gestionnaire {  get; private set;}

        public GestionnaireVM(Gestionnaire gestionnaire)
        {
            _gestionnaire = gestionnaire;

            _gestionnaire.PropertyChanged += _gestionnaire_PropertyChanged;
        }

        private void _gestionnaire_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_gestionnaire))
                _gestionnaire = _gestionnaire as Gestionnaire;
        }
    }
}
