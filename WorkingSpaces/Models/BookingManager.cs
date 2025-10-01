public class BookingManager
{
    public List<Space> Spaces { get; } = new List<Space>();
    public List<Booking> Bookings { get; } = new List<Booking>();

    public bool IsAvailable(Space space, DateTimeOffset start, DateTimeOffset end)
    {
        if (start.Hour < 8 || end.Hour > 23 || end <= start || start < DateTimeOffset.Now)
            return false;
        if ((end - start).TotalMinutes < 15)
            return false;

        return !Bookings.Any(b =>
            b.Space.RoomId == space.RoomId && start < b.EndTime && end > b.StartTime);
    }

    public Booking? CreateBooking(Space space, string userEmail, DateTimeOffset start, DateTimeOffset end)
    {
        if (!IsAvailable(space, start, end))
            return null;

        var booking = new Booking
        {
            BookingId = Bookings.Count + 1,
            Space = space,
            UserEmail = userEmail,
            StartTime = start,
            EndTime = end
        };

        Bookings.Add(booking);
        return booking;
    }
}