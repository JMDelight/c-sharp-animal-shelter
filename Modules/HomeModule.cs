using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace AnimalShelter
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<Species> allSpecies = Species.GetAll();
        return View["index.cshtml", allSpecies];
      };
      Get["species/new"] = _ => View["species_form.cshtml"];

      Post["species/new"] = _ => {
        Species newSpecies = new Species(Request.Form["species-name"]);
        newSpecies.Save();
        List<Species> allSpecies = Species.GetAll();
        return View["index.cshtml", allSpecies];
      };

      Get["species/{id}"] = parameters => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        var selectedSpecies = Species.Find(parameters.id);
        var speciesAnimals = selectedSpecies.GetAnimal();
        model.Add("species", selectedSpecies);
        model.Add("animals", speciesAnimals);
        return View["species.cshtml", model];
      };

      Get["animal/new/{id}"] = parameters => {
        Species selectedSpecies = Species.Find(parameters.id);

        return View["animal_form.cshtml", selectedSpecies];
      };

      Post["animal/new/{id}"] = parameters => {
        Animal newAnimal = new Animal(Request.Form["animal-name"], Request.Form["animal-breed"], Request.Form["animal-gender"], Request.Form["animal-age"], Request.Form["animal-species"]);
        newAnimal.Save();
        List<Species> allSpecies = Species.GetAll();
        return View["index.cshtml", allSpecies];
      };

      // Post["/animals"] = _ => {
      //   List<Animal
      // };
    }
  }
}
