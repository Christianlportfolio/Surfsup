using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfProjekt.Models
{
    [Display(Name = "Navn")]
    public class Equipment
    {
        public int EquipmentID { get; set; }

        public string Name { get; set; }

        public SUPboard SUPboard { get; set; }
        public int SUPBoardID { get; set; }
    }
}
