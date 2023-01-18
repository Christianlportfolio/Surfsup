namespace SurfProjekt.Models.ViewModels
{
    public class BoardHierarchy
    {
        public Equipment Equipment { get; set; }
        public PaginatedList<Boards> Boards { get; set; }
        public IEnumerable<SUPboard> SUPboards { get; set; }

        public Boards Board { get; set; }




    }
}
