
using System.ComponentModel.DataAnnotations;

namespace SurfProjekt.Models
{
    public class Lease
    {
        [ConcurrencyCheck]
        public int LeaseID { get; set; }
        public DateTime Date { get; set; }
        public int TimeFrame { get; set; }
        private DateTime endTime;
        public DateTime EndTime
        {
            get
            {
                return Date.AddHours(TimeFrame);
            }
            set
            {
                endTime = value;
            }
        }
        public int BoardID { get; set; }
        public Boards Board { get; set; }

        public string? UserID { get; set; }

        //Det er en tracking property, som skal bruges til databasen for at se om en rækkes værdier har ændret sig.
        [Timestamp]
        public byte[] RowVersion { get; set; }


        public Lease()
        {

        }
        public Lease(int boardId, string userId, DateTime date, int timeFrame)
        {
            BoardID = boardId;
            UserID = userId;
            Date = date;
            TimeFrame = timeFrame;
        }
    }
}
