using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServer.Models
{
    [Table("Category")]
    public class CategoryList
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
