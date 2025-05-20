using PartageDepense.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Linq;
using PartageDepense.ViewModel;
using System.Windows.Threading;
using System.Windows.Threading;

namespace TestsUnitaires
{
    [TestClass]
    public sealed class TestsModel
    {
        Gestionnaire gestionnaire = new Gestionnaire();
        private Activite _activite;
        private Participant _participant1;
        private Participant _participant2;
        private Participant _participant3;

        [TestInitialize]
        public void Setup()
        {
            //Initialisation des participants
            gestionnaire.AjouterParticipant("Jacob", "St-Hilaire");
            gestionnaire.AjouterParticipant("Oussama", "Aziz");
            gestionnaire.AjouterParticipant("Gaetan", "Rhamala");
            gestionnaire.AjouterParticipant("Yann", "Pokam");
            gestionnaire.AjouterParticipant("Patrick", "Simard");
            gestionnaire.AjouterParticipant("Eric", "Neron");
            gestionnaire.AjouterParticipant("Ismael", "Harvey");
            gestionnaire.AjouterParticipant("Nouveau", "Nouveau");

            //Initialisation des activités
            gestionnaire.AjouterActivite("Camping Car");
            gestionnaire.AjouterActivite("SnowBoard");
            gestionnaire.AjouterActivite("Footing");
            gestionnaire.AjouterActivite("Bowling");

            // Création d'une activité de test
            _activite = new Activite("Test Activity");

            // Création des participants de test
            _participant1 = new Participant("Doe", "John");
            _participant2 = new Participant("Smith", "Jane");
            _participant3 = new Participant("Johnson", "Bob");

            // Ajout des participants à l'activité
            _activite.AjouterParticipant(_participant1);
            _activite.AjouterParticipant(_participant2);
            _activite.AjouterParticipant(_participant3);
        }

        [TestMethod]
        public void AjouterParticipant()
        {
            gestionnaire.LesParticipants?.Count.Should().Be(8);
        }

        [TestMethod]
        public void SupprimerParticipant()
        {
            if(gestionnaire.LesParticipants is not null)
            {
                gestionnaire.SupprimerParticipant(gestionnaire.LesParticipants[0]);
                gestionnaire.SupprimerParticipant(gestionnaire.LesParticipants[1]);
                gestionnaire.SupprimerParticipant(gestionnaire.LesParticipants[2]);
            }
            
            gestionnaire.LesParticipants?.Count.Should().Be(5);
        }

        [TestMethod]
        public void AjouterActivite()
        {
            gestionnaire.LesParticipants?.Count.Should().Be(8);
        }

        [TestMethod]
        public void SupprimerActivite()
        {
            gestionnaire.SupprimerParticipant(new Participant("Eric", "Neron"));
            gestionnaire.AjouterParticipant("Ismael", "Harvey");
            gestionnaire.AjouterParticipant("Nouveau", "Nouveau");

            gestionnaire.LesParticipants?.Count.Should().Be(8);
        }

        [TestMethod]
        public void AjouterSauvegarde()
        {
            gestionnaire.LesParticipants?.Count.Should().Be(8);
        }

        [TestMethod]
        public void SupprimerSauvegarde()
        {
            gestionnaire.SupprimerParticipant(new Participant("Eric", "Neron"));
            gestionnaire.AjouterParticipant("Ismael", "Harvey");
            gestionnaire.AjouterParticipant("Nouveau", "Nouveau");

            gestionnaire.LesParticipants?.Count.Should().Be(8);
        }
    }
}
