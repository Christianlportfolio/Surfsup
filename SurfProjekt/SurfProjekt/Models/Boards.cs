using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfProjekt.Models
{
    public class Boards
    {
        public int Id { get; set; }

        [Display(Name = "Navn")]
        public string Name { get; set; }

        [Range(0, 100)]
        [Display(Name = "Længde (feet)")]
        public double Length { get; set; }

        [Display(Name = "Bredde (inches)")]
        public double Width { get; set; }

        [Display(Name = "Tykkelse (inches)")]
        public double Thickness { get; set; }

        [Display(Name = "Volumen (L)")]
        public double Volume { get; set; }

        public string Type { get; set; }

        [Display(Name = "Pris (€)")]
        [Column(TypeName= "decimal(18,2")]
        public decimal Price { get; set; }

        [Display(Name = "")]
        public string? Image { get; set; }


        //[ConcurrencyCheck]

        public bool IsPremium { get; set; }


        private bool isRented;
        public bool IsRented
        {
            get
            {
                if (leases != null)
                {
                    foreach (Lease l in leases)
                    {
                        if (DateTime.Now > l.Date && DateTime.Now < l.EndTime)
                            return true;
                    }
                }
                return false;
            }
            set
            {
                isRented = value;
            }
        }

        [Timestamp]
        public byte[] RowVersionBoards { get; set; }

        public ICollection<Lease> leases { get; set; }

        public Boards()
        {

        }
    }

}
