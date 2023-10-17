using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Models
{
    public class Item
    {
        public static int count = 1;
        public int id = count;
        [Required] public string name { get; set; }
        [Required] public double price { get; set; }
        [Required] public int quantity { get; set; }
        public static void increment() { count++; }
    }
}