namespace WordList.Api.Attributes;

public class AttributeDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }
    public required string Display { get; set; }

}