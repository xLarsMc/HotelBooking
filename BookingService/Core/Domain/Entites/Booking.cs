using Domain.Enums;
using Action = Domain.Enums.Action;

namespace Domain.Entites
{
    public class Booking
    {
        public Booking () { this.Status = Status.Created; }
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Room Room { get; set; }
        public Guest Guest { get; set; }
        private Status Status { get; set; }
        //Encapsulação do status, não permitindo sua alteração diretamente no objeto, podendo apenas ser acessado via método CurrentStatus
        public Status CurrentStatus { get { return this.Status; } }

        //StateMachine design pattern - Mudança desse estado definido já na classe, encapsulando e evitando muudanças inesperadas e foras de lógica.
        public void ChangeState(Action action)
        {
            this.Status = (this.Status, action) switch
            {
                (Status.Created,  Action.Paying) => Status.Paid,
                (Status.Created,  Action.Cancelling) => Status.Canceled,
                (Status.Paid,     Action.Finishing) => Status.Finished,
                (Status.Paid,     Action.Refounding) => Status.Refounded,
                (Status.Canceled, Action.Reopenning) => Status.Created,
                _ => this.Status
            };
        }
    }
}
