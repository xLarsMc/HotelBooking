using Domain.ValueObjects;

namespace Domain.Entites
{
    public class Room
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public Price Price { get; set; }
        public bool isAvailable
        {
            get
            {
                if (!this.InMaintenance || this.HasGuest) return false;
                
                return true;
            }
        }

        public bool HasGuest 
        {
            get { return true; }
        }
    }
}
