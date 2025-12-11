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
        
        // Добавь это свойство
        public int UserId { get; set; }
    }

    // Убедись что CreateValeraDto и UpdateValeraDto тоже есть
    public class CreateValeraDto
    {
        public int Health { get; set; }
        public int Alcohol { get; set; }
        public int Joy { get; set; }
        public int Fatigue { get; set; }
        public decimal Money { get; set; }
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