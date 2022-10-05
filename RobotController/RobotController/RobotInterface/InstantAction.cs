namespace SafelogSimulator;

public class InstantAction
{
    public string ActionName { get; set; }
    public int ActionId { get; set; }
    public string BlockingType { get; set; }
    public List<ActionParameter> ActionParameters { get; set; }
}