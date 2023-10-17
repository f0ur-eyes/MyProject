using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyProject.Models
{
    public class User
    {
        public static int count = 1;
        public int id = count;
        [Required] public string firstName { get; set; }
        [Required] public string lastName { get; set; }
        [Required] public string email { get; set; }
        public string token;
        [Required] public int phoneNumber { get; set; }
        public void increment() { count++; }
    }
}