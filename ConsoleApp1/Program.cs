namespace ConsoleApp1
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // TV, Lights, PC,
        //Refrigerator,...
}
    public class EnergySource
    {
        public int Id { get; set; }
        // property does not exists -> // Battery, Grid, Solar
    }
    public class ConsuptionLog
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public double UsedEnergy { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
