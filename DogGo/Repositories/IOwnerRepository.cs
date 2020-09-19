using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        void AddOwner(Owner owner);
        void DeleteOwner(int ownerId);
        Owner GetOwnerByEmail(string email);
        Owner GetOwnerById(int id);
        void UpdateOwner(Owner owner);
        List<Owner> GetAll();
    }
}