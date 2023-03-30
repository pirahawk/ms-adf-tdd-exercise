namespace Ms.Tdd.Adf.Tests.Specs.Models
{
    public record MatchRow
    {
        public string? Home { get; set; }
        public string? Away { get; set; }
        public int ScoreHome { get; set; }
        public int ScoreAway { get; set; }
        public string? Result { get; set; }

    }
}