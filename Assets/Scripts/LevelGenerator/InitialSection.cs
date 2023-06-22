using System.Linq;

namespace LevelGenerator.Scripts
{
    public class InitialSection : Section
    {
        protected override void RegisterInGenerator() => LevelGenerator.RegisterInitialSection(this);

        protected override void GenerateAnnexes() => GenerateSection(Elements.ContainedElements.First().ExitSpots.First());
    }
}