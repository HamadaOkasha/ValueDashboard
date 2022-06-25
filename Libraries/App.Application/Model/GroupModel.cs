using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Model
{
    public class GroupModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string Name { get; set; }
        [Required(ErrorMessage = "FieldRequired")]
        public string NameAr { get; set; }
        public bool IsMembers { get; set; }

        public string[] AdminPagesIds { get; set; }
        public SelectList AdminPagesList { get; set; }
    }
}
