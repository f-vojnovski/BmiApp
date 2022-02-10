using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BMI
    {
        public Guid Id { get; set; }
        public String email { get; set; }
        public Double Weight { get; set; }

        public Double Height { get; set; }

        public Double Bmi { get; set; }
    }
}
