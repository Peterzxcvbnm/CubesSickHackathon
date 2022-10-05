namespace SafelogSimulator;

public class RootDto
{
    public int HeaderId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Version { get; set; }
    public string Manufacturer { get; set; }
    public string SerialNumber { get; set; }
    public List<InstantAction> InstantActions { get; set; }
}