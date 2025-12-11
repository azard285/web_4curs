using System;

namespace ValeraApi.Models
{
    public class Valera
    {
        public int Id { get; set; }
        public int Health { get; set; }
        public int Alcohol { get; set; }
        public int Joy { get; set; }
        public int Fatigue { get; set; }
        public decimal Money { get; set; }
        
        // Добавляем связь с пользователем
        public int UserId { get; set; }
        public User? User { get; set; } // Делаем nullable

        public Valera() { }

        public Valera(int health = 100, int alcohol = 0, int joy = 0, int fatigue = 0, decimal money = 0)
        {
            Health = health;
            Alcohol = alcohol;
            Joy = joy;
            Fatigue = fatigue;
            Money = money;
            ClampValues();
        }

        private void ClampValues()
        {
            Health = Math.Max(0, Math.Min(100, Health));
            Alcohol = Math.Max(0, Math.Min(100, Alcohol));
            Joy = Math.Max(-10, Math.Min(10, Joy));
            Fatigue = Math.Max(0, Math.Min(100, Fatigue));
            if (Money < 0) Money = 0;
        }

        public bool GoToWork()
        {
            if (Alcohol >= 50 || Fatigue >= 10)
                return false;

            Joy -= 5;
            Alcohol -= 30;
            Money += 100;
            Fatigue += 70;
            ClampValues();
            return true;
        }

        public void ContemplateNature()
        {
            Joy += 1;
            Alcohol -= 10;
            Fatigue += 10;
            ClampValues();
        }

        public void DrinkWineAndWatchSeries()
        {
            if (Money < 20) return;

            Joy -= 1;
            Alcohol += 30;
            Fatigue += 10;
            Health -= 5;
            Money -= 20;
            ClampValues();
        }

        public void GoToBar()
        {
            if (Money < 100) return;

            Joy += 1;
            Alcohol += 60;
            Fatigue += 40;
            Health -= 10;
            Money -= 100;
            ClampValues();
        }

        public void DrinkWithMarginals()
        {
            if (Money < 150) return;

            Joy += 5;
            Health -= 80;
            Alcohol += 90;
            Fatigue += 80;
            Money -= 150;
            ClampValues();
        }

        public void SingInMetro()
        {
            Joy += 1;
            Alcohol += 10;
            Money += 10;

            int initialAlcohol = Alcohol - 10;
            if (initialAlcohol > 40 && initialAlcohol < 70)
                Money += 50;

            Fatigue += 20;
            ClampValues();
        }

        public void Sleep()
        {
            int initialAlcohol = Alcohol;

            if (initialAlcohol < 30)
                Health += 90;

            if (initialAlcohol > 70)
                Joy -= 3;

            Alcohol -= 50;
            Fatigue -= 70;
            ClampValues();
        }
    }
}

