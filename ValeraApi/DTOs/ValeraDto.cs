namespace ValeraApi.DTOs
{
    public class ValeraDto
    {
        public int Id { get; set; }
        public int Health { get; set; }
        public int Alcohol { get; set; }
        public int Joy { get; set; }
        public int Fatigue { get; set; }
        public decimal Money { get; set; }
    }

    public class CreateValeraDto
    {
        public int Health { get; set; } = 100;
        public int Alcohol { get; set; } = 0;
        public int Joy { get; set; } = 0;
        public int Fatigue { get; set; } = 0;
        public decimal Money { get; set; } = 0;
    }

    public class UpdateValeraDto
    {
        public int Health { get; set; }
        public int Alcohol { get; set; }
        public int Joy { get; set; }
        public int Fatigue { get; set; }
        public decimal Money { get; set; }
    }
}