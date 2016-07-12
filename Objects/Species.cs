using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AnimalShelter
{
  public class Species
  {
    private string _name;
    private int _id;

    public Species(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public override bool Equals(System.Object otherSpecies)
    {
      if (!(otherSpecies is Species))
      {
        return false;
      }
      else
      {
        Species newSpecies = (Species) otherSpecies;
        bool idEquality = this.GetId() == newSpecies.GetId();
        bool nameEquality = this.GetName() == newSpecies.GetName();
        return (idEquality && nameEquality);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public static List<Species> GetAll()
    {
      List<Species> allSpecies = new List<Species>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM species;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int speciesId = rdr.GetInt32(0);
        string speciesName = rdr.GetString(1);
        Species newSpecies = new Species(speciesName, speciesId);
        allSpecies.Add(newSpecies);
      }

      // foreach(Species xyz in allSpecies)
      // {
      //   System.Console.WriteLine("name=" + xyz.GetName());
      //   System.Console.WriteLine("id=" + xyz.GetId());
      // }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return allSpecies;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO species (name) OUTPUT INSERTED.id VALUES (@SpeciesName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@SpeciesName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM species;", conn);
      cmd.ExecuteNonQuery();
    }

    public List<Animal> GetAnimal()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM animals WHERE species_id = @SpeciesId;", conn);
      SqlParameter speciesIdParameter = new SqlParameter();
      speciesIdParameter.ParameterName = "@SpeciesId";
      speciesIdParameter.Value = this.GetId();
      cmd.Parameters.Add(speciesIdParameter);
      rdr = cmd.ExecuteReader();

      List<Animal> animals = new List<Animal> {};
      while(rdr.Read())
      {
        int animalId = rdr.GetInt32(0);
        string animalName = rdr.GetString(1);
        string animalBreed = rdr.GetString(2);
        string animalGender = rdr.GetString(3);
        int animalAge = rdr.GetInt32(4);
        Animal newAnimal = new Animal(animalName, animalBreed, animalGender, animalAge, this.GetId());
        animals.Add(newAnimal);
        System.Console.WriteLine("Name: " + animalName);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return animals;
    }

    public static Species Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM species WHERE id = @SpeciesId;", conn);
      SqlParameter speciesIdParameter = new SqlParameter();
      speciesIdParameter.ParameterName = "@SpeciesId";
      speciesIdParameter.Value = id.ToString();
      cmd.Parameters.Add(speciesIdParameter);
      rdr = cmd.ExecuteReader();

      int foundSpeciesId = 0;
      string foundSpeciesDescription = null;

      while(rdr.Read())
      {
        foundSpeciesId = rdr.GetInt32(0);
        foundSpeciesDescription = rdr.GetString(1);
      }
      Species foundSpecies = new Species(foundSpeciesDescription, foundSpeciesId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundSpecies;
    }
  }
}
