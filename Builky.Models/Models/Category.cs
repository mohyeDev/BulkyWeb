﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Builky.Models.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("Display Order")]

        [Range(1,100 , ErrorMessage ="Display Order Must Be Between 1-100")]
        public int DisplayOrder { get; set; }

    }
}
