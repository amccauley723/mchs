namespace MarshfieldClinic.Models
{
    public class Doctors
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string NiceUrl { get; set; }
        public string NodeAliasType { get; set; }
    }

    public class Locations
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string NiceUrl { get; set; }
        public string NodeAliasType { get; set; }
    }

    public class Specialties
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string NiceUrl { get; set; }
        public string NodeAliasType { get; set; }
    }

    public class Result
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public string NiceUrl { get; set; }
        public string NodeAliasType { get; set; }
    }
}