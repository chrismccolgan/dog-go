using DogGo.Models;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public interface IDogRepository
    {
        void AddDog(Dog newDog);
        List<Dog> GetAllDogs();
        List<Dog> GetDogsByOwnerId(int ownerId);
    }
}