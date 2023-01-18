
namespace SurfProjekt.Models
{
    public class Lease
    {
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
        public Boards? Board { get; set; }
        public string? UserID { get; set; }
        
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
