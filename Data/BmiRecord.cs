using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BmiRecord
    {
        [Key]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }

        public double Bmi { get; set; }
    }
}
