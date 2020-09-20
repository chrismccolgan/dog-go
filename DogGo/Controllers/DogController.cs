using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    public class DogController : Controller
    {
        private readonly IDogRepository _dogRepository;
        private readonly IOwnerRepository _ownerRepository;

        public DogController(IDogRepository dogRepository, IOwnerRepository ownerRepository)
        {
            _dogRepository = dogRepository;
            _ownerRepository = ownerRepository;
        }

        // GET: DogController
        public ActionResult Index()
        {
            List<Dog> dogs = _dogRepository.GetAllDogs();
            return View(dogs);
        }


        //// LOOK AT THIS
        //public ActionResult Create()
        //{
        //    // We use a view model because we need the list of Owners in the Create view
        //    DogFormViewModel vm = new DogFormViewModel()
        //    {
        //        Dog = new Dog(),
        //        Owners = _ownerRepository.GetAll(),
        //    };

        //    return View(vm);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Dog dog)
        //{
        //    try
        //    {
        //        // LOOK AT THIS
        //        //  Let's save a new dog
        //        //  This new dog may or may not have Notes and/or an ImageUrl
        //        _dogRepository.AddDog(dog);

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        // LOOK AT THIS
        //        //  When something goes wrong we return to the view
        //        //  BUT our view expects a DogFormViewModel object...so we'd better give it one
        //        DogFormViewModel vm = new DogFormViewModel()
        //        {
        //            Dog = dog,
        //            Owners = _ownerRepository.GetAll(),
        //        };

        //        return View(vm);
        //    }
        //}
    }
}