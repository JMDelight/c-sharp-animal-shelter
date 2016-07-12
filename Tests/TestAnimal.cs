using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AnimalShelter
{
  public class AnimalTest : IDisposable
  {
    public AnimalTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=animals_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DataBaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Animal.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Save_ItemAddedToDatabase()
    {
      //Arrange
      Animal newAnimal = new Animal("Bob", "ballpython", "bob", 4, 1);
      newAnimal.Save();

      //Act
      List<Animal> testList = Animal.GetAll();

      // Assert
      Assert.Equal(1, testList.Count);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Animal firstAnimal = new Animal("Bob", "ballpython", "bob", 4, 1);
      Animal secondAnimal = new Animal("Bob", "ballpython", "bob", 4, 1);

      //Assert
      Assert.Equal(firstAnimal, secondAnimal);
     }


    public void Dispose()
    {
      Animal.DeleteAll();
    }
  }
}
