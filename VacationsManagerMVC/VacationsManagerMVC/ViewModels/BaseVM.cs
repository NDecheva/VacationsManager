using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace VacationsManagerMVC.ViewModels
{
    public class BaseVM
    {
        public int Id { get; set; }

        [DisplayName("Created At")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DisplayName("Last Updated")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }
    }
}
