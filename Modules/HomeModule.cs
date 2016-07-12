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

      // Post["/animals"] = _ => {
      //   List<Animal
      // };
    }
  }
}
