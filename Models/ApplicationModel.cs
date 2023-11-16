using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;
using Microsoft.OData.ModelBuilder;

namespace ODataSample.Models
{
    public class ApplicationModel
    {

        public ApplicationModel()
        {
            Permissions =  new HashSet<PermissionModel>();
        }

              
        [Ignore]
        public int ID { get; set; }

        [Required]
        [MaxLength(255), MinLength(3)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Ignore]
        public string CreatedBy { get; set; } 

        
        [Ignore]
        public DateTime ModifiedDate { get; set; }

        [AutoExpand]
        public  ICollection<PermissionModel> Permissions { get; set; }
    }
}
