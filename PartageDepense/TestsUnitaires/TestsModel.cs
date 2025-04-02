using PartageDepense.Model;
using FluentAssertions;

namespace TestsUnitaires
{
    [TestClass]
    public sealed class TestsModel
    {
        Gestionnaire gestionnaire = new Gestionnaire();

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
