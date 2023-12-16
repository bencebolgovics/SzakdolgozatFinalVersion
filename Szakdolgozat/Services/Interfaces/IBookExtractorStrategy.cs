using System.Threading.Tasks;
using Szakdolgozat.Models;

namespace Szakdolgozat.Services.Strategies
{
    public interface IBookManipulatorStrategy
    {
        public void ExtractingAndSaving(string path);
        public void DeleteBookResources(Book book);
    }
}
