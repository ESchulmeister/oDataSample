using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace ODataSample.Data
{
    //DB 
    public partial class AppPermission
    {
        [Key]
        public int ApId { get; set; }
        public int AppId { get; set; }
        public string PermName { get; set; }
        public bool? ApActive { get; set; } = true;

        public string ApCreatedBy { get; set; }
        public DateTime ApCreatedDate { get; set; }
        public string ApModifiedBy { get; set; }
        public DateTime ApModifiedDate { get; set; }

        public virtual Application App { get; set; }
    }
}
