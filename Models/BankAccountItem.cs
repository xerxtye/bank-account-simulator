namespace BankAccountApi.Models;

public class BankAccountItem
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public long Balance { get; set; }
    public string? Secret { get; set; }
}
