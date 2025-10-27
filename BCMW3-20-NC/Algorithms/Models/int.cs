namespace Algorithms.Models
{
    public class @int
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public byte Cylinder { get; set; }
        public float Engine { get; set; }
        public string Drive { get; set; }
        public string Transmission { get; set; }
        public byte City { get; set; }
        public byte Combined { get; set; }
        public byte Highway { get; set; }

        public static @int Parse(string input)
        {
            string[] data = input.Split(',');

            if (data.Length != 9)
                throw new FormatException("Invalid input");

            @int result = new();

            result.Make = data[0];
            result.Model = data[1];
            result.Cylinder = byte.Parse(data[2]);
            result.Engine = float.Parse(data[3]);
            result.Drive = data[4];
            result.Transmission = data[5];
            result.City = byte.Parse(data[6]);
            result.Combined = byte.Parse(data[7]);
            result.Highway = byte.Parse(data[8]);

            return result;
        }

    }
}
