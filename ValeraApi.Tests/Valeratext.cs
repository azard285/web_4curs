using Xunit;
using ValeraApi.Models;

namespace ValeraApi.Tests
{
    public class ValeraTests
    {
        private Valera CreateDefaultValera()
        {
            
            return new Valera(health: 100, alcohol: 0, joy: 0, fatigue: 0, money: 1000);
        }

        [Fact]
        public void GoToWork_ValidConditions_ChangesStateCorrectly()
        {
           
            var valera = new Valera(health: 100, alcohol: 40, joy: 0, fatigue: 5, money: 1000);

           
            bool success = valera.GoToWork();

          
            Assert.True(success);
            Assert.Equal(-5, valera.Joy);
            Assert.Equal(10, valera.Alcohol); 
            Assert.Equal(1100, valera.Money);
            Assert.Equal(75, valera.Fatigue); 
        }

        [Fact]
        public void GoToWork_InvalidAlcohol_ReturnsFalseNoChange()
        {
           
            var valera = new Valera(alcohol: 60);

           
            bool success = valera.GoToWork();

         
            Assert.False(success);
            Assert.Equal(0, valera.Joy);
            Assert.Equal(60, valera.Alcohol);
            Assert.Equal(0, valera.Money);
            Assert.Equal(0, valera.Fatigue);
        }

        [Fact]
        public void ContemplateNature_ChangesStateCorrectly()
        {
           
            var valera = CreateDefaultValera();

            
            valera.ContemplateNature();

            
            Assert.Equal(1, valera.Joy);
            Assert.Equal(0, valera.Alcohol); 
            Assert.Equal(10, valera.Fatigue);
        }

        [Fact]
        public void DrinkWineAndWatchSeries_ChangesStateCorrectly()
        {
            
            var valera = CreateDefaultValera();

           
            valera.DrinkWineAndWatchSeries();

          
            Assert.Equal(-1, valera.Joy);
            Assert.Equal(30, valera.Alcohol);
            Assert.Equal(10, valera.Fatigue);
            Assert.Equal(95, valera.Health);
            Assert.Equal(980, valera.Money);
        }

        [Fact]
        public void DrinkWineAndWatchSeries_NotEnoughMoney_NoChange()
        {
        
            var valera = new Valera(money: 10);

            
            valera.DrinkWineAndWatchSeries();

           
            Assert.Equal(0, valera.Joy); 
            Assert.Equal(0, valera.Alcohol);
            Assert.Equal(0, valera.Fatigue);
            Assert.Equal(100, valera.Health);
            Assert.Equal(10, valera.Money);
        }

        [Fact]
        public void GoToBar_ChangesStateCorrectly()
        {
            
            var valera = CreateDefaultValera();

          
            valera.GoToBar();

            Assert.Equal(1, valera.Joy);
            Assert.Equal(60, valera.Alcohol);
            Assert.Equal(40, valera.Fatigue);
            Assert.Equal(90, valera.Health);
            Assert.Equal(900, valera.Money);
        }

        [Fact]
        public void DrinkWithMarginals_ChangesStateCorrectly()
        {
            var valera = CreateDefaultValera();

           
            valera.DrinkWithMarginals();

          
            Assert.Equal(5, valera.Joy);
            Assert.Equal(20, valera.Health);
            Assert.Equal(90, valera.Alcohol);
            Assert.Equal(80, valera.Fatigue);
            Assert.Equal(850, valera.Money);
        }

        [Fact]
        public void SingInMetro_NoBonus_ChangesStateCorrectly()
        {
          
            var valera = new Valera(alcohol: 20, money: 1000);

           
            valera.SingInMetro();

            
            Assert.Equal(1, valera.Joy);
            Assert.Equal(30, valera.Alcohol); 
            Assert.Equal(20, valera.Fatigue);
            Assert.Equal(1010, valera.Money);
        }

        [Fact]
        public void SingInMetro_WithBonus_ChangesStateCorrectly()
        {
           
            var valera = new Valera(alcohol: 50, money: 1000);

           
            valera.SingInMetro();

            
            Assert.Equal(1, valera.Joy);
            Assert.Equal(60, valera.Alcohol);
            Assert.Equal(20, valera.Fatigue);
            Assert.Equal(1060, valera.Money);
        }

        [Fact]
        public void Sleep_LowAlcohol_HealsHealth()
        {
            
            var valera = new Valera(health: 20, alcohol: 20, fatigue: 80);

            
            valera.Sleep();

           
            Assert.Equal(100, valera.Health); 
            Assert.Equal(0, valera.Joy);
            Assert.Equal(0, valera.Alcohol); 
            Assert.Equal(10, valera.Fatigue); 
        }

        [Fact]
        public void Sleep_HighAlcohol_DecreasesJoy()
        {
           
            var valera = new Valera(alcohol: 80);

           
            valera.Sleep();

           
            Assert.Equal(-3, valera.Joy);
            Assert.Equal(30, valera.Alcohol); 
        }

        [Fact]
        public void ClampValues_AfterNegative_ClampsCorrectly()
        {
            // Arrange
            var valera = new Valera(health: -10, alcohol: 110, joy: 15, fatigue: -5, money: -100);

            // Act
            valera.ContemplateNature();

            // Assert
            Assert.Equal(0, valera.Health); 
            Assert.Equal(90, valera.Alcohol); 
            Assert.Equal(10, valera.Joy);
            Assert.Equal(10, valera.Fatigue); 
            Assert.Equal(0, valera.Money);
        }
    }
}