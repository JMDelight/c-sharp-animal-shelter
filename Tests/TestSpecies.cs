using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AnimalShelter
{
    public class SpeciesTest : IDisposable
  {
    public SpeciesTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=animals_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_SpeciesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Species.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Species firstSpecies = new Species("Snake");
      Species secondSpecies = new Species("Snake");

      //Assert
      Assert.Equal(firstSpecies, secondSpecies);
     }

    [Fact]
    public void Test_Save_SavesSpeciesToDatabase()
    {
      //Arrange
      Species testSpecies = new Species("Snake");
      testSpecies.Save();

      //Act
      List<Species> result = Species.GetAll();
      List<Species> testList = new List<Species>{testSpecies};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToSpeciesObject()
    {
      //Arrange
      Species testSpecies = new Species("Snake");
      testSpecies.Save();

      //Act
      Species savedSpecies = Species.GetAll()[0];

      int result = savedSpecies.GetId();
      int testId = testSpecies.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsSpeciesInDatabase()
    {
      //Arrange
      Species testSpecies = new Species("Snake");
      testSpecies.Save();

      //Act
      Species foundSpecies = Species.Find(testSpecies.GetId());

      //Assert
      Assert.Equal(testSpecies, foundSpecies);
    }

    [Fact]
    public void Test_GetSpecies_RetrievesAllAnimalsWithinSpecies()
    {
      Species testSpecies = new Species("Snake");
      testSpecies.Save();

      Animal firstAnimal = new Animal("Bob", "ballpython", "bob", 4, testSpecies.GetId());
      firstAnimal.Save();
      Animal secondAnimal = new Animal("Bill", "RubberBoa", "bob", 7, testSpecies.GetId());
      secondAnimal.Save();


      List<Animal> testSpeciesList = new List<Animal> {firstAnimal, secondAnimal};
      List<Animal> resultSpeciesList = testSpecies.GetAnimal();

      Assert.Equal(testSpeciesList, resultSpeciesList);
    }

    public void Dispose()
    {
      Animal.DeleteAll();
      Species.DeleteAll();
    }
  }
}
