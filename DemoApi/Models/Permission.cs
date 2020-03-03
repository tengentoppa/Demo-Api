using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApi.Models
{
    public class Permission
    {
        [Key]
        [Required]
        public int PermissionId { get; set; }
        [Required]
        [StringLength(100)]
        public string AccessibleContent { get; set; }
    }
}
