public class Booking
{
    public int BookingId { get; set; }
    public required Space Space { get; set; }
    public required string UserEmail { get; set; } 
    public required DateTimeOffset StartTime { get; set; }
    public required DateTimeOffset EndTime { get; set; }
    public override string ToString() =>
        $"Booking #{BookingId}: {Space.ToString()} by {UserEmail} from {StartTime:HH:mm} to {EndTime:HH:mm} on {StartTime:dd.MM.yyyy}";
}