namespace LibraryModsen.Domain.Models;

public class AppFile
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public byte[] Data { get; set; } = [];
}
