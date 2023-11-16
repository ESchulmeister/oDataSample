using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODataSample.Models
{
    [ComplexType]
    public class PermissionModel
    {
        public int ApplicationID { get; set; }

        public int PermissionID { get; set; }

        public string Permission { get; set; }

        public bool? IsActive { get; set; } = true;

        public   bool HasValue
        {
            get
            {
                return (Permission != null);
            }
        }


    }
}
