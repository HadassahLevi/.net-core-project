namespace music.Models;

public class MusicalInstruments
{
    public static int Count = 0;

    public int Id { get; set; }

    public string? Name { get; set; }

    public double Price { get; set; }

    public bool IsElectric { get; set; }

    public int UserId { get; set; }

}