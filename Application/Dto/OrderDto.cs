namespace Application.Dto;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PartId { get; set; }
    
    public string ProductName { get; set; }
    public string PartName { get; set; }
}