
[Flags] 
public enum Equipment
{
    None = 0,         
    TV = 1,
    Projector = 2,
    Board = 4,
    Computers = 8,
}

public class Space
{
    public int RoomId { get; set; }
    public int NumberOfSeats { get; set; }
    public Equipment AvailableEquipment { get; set; }
}