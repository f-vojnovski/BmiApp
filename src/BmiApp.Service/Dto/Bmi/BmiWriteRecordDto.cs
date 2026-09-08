namespace BmiApp.Service.Dto.Bmi
{
    public class BmiWriteRecordDto
    {
        public string Email { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }

        public double Bmi { get; set; }
    }
}
