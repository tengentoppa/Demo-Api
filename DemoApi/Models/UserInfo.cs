using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApi.Models
{
    public class UserInfo
    {
        [Key]
        [Required]
        public int UserInfoId { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(200)]
        public string Memo { get; set; }
    }
}
