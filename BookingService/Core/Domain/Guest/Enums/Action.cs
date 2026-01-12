namespace Domain.Guest.Enums
{
    public enum Action
    {
        Paying = 0,
        Finishing = 1, //Paid and Used
        Cancelling = 2, //Can never be paid
        Refounding = 3, //Paid then refounded
        Reopenning = 4, //Only canceleds
    }
}
