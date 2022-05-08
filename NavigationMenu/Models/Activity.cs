using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NavigationMenu.Models
{
    public class Activity : BaseModel
    {
        private int _id;
        private string _name;
        private string _createdOn;
        private string _createdBy;
        private string _lastModifiedOn;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string CreatedOn
        {
            get => _createdOn;
            set => SetProperty(ref _createdOn, value);
        }
        public string CreatedBy
        {
            get => _createdBy;
            set => SetProperty(ref _createdBy, value);
        }
        public string LastModifiedOn
        {
            get => _lastModifiedOn;
            set => SetProperty(ref _lastModifiedOn, value);
        }

    }
}
