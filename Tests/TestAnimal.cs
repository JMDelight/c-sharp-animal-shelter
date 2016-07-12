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
    public void Test_ItemAddedToDatabase()
    {
      //Arrange
      Animal newAnimal = new Animal("Bob", "ballpython", "bob", 4);
      newAnimal.Save();

      //Act
      List<Animal> testList = Animal.GetAll();

      // Assert
      Assert.Equal(1, testList.Count);
    }


    public void Dispose()
    {
      Animal.DeleteAll();
    }
  }
}
