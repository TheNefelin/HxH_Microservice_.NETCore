namespace ClassLibrary.Core.Entities;

public class Hunter
{
    public required int Id_Hunter { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required int Age { get; set; }
    public required string Origin { get; set; } = string.Empty;
}
