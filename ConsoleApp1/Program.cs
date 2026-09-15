using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    public class HomeEnergyDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<EnergySource> EnergySources { get; set; }
        public DbSet<ConsumptionLog> ConsumptionLogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=HomeEnergy.db");
        }
    }

    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; //TV, Lights, PC, Refrigerator, ...

        public int EnergySourceId { get; set; }
        public EnergySource EnergySource { get; set; }

        public List<ConsumptionLog> ConsumptionLogs { get; set; } =
                new List<ConsumptionLog>();
    }

    public class EnergySource
    {
        public int Id { get; set; }
        //property does not exists -> //Battery, Grid, Solar
        public string Type { get; set; } = string.Empty;

        public List<Device> Devices { get; set; } = new List<Device>();
        public List<ConsumptionLog> ConsumptionLogs { get; set; } =
                new List<ConsumptionLog>();
    }

    public class ConsumptionLog
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public double UsedEnergy { get; set; }

        public int DeviceId { get; set; }
        public Device Device { get; set; }

        public int EnergySourceId { get; set; }
        public EnergySource EnergySource { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new HomeEnergyDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var es1 = new EnergySource { Type = "Grid" };
            var es2 = new EnergySource { Type = "Solar" };
            var es3 = new EnergySource { Type = "Battery" };

            context.EnergySources.Add(es1);
            context.EnergySources.Add(es2);
            context.EnergySources.Add(es3);
            context.SaveChanges();

            var device1 = new Device{Name = "TV", EnergySourceId = es1.Id};
            var device2 = new Device{Name = "PC", EnergySourceId = es1.Id};
            var device3 = new Device{Name = "Refrigerator", EnergySourceId = es2.Id};

            context.Devices.Add(device1);
            context.Devices.Add(device2);
            context.Devices.Add(device3);
            context.SaveChanges();

            var log1 = new ConsumptionLog
            {
                DateTime = DateTime.Now,
                UsedEnergy = 0.5,
                DeviceId = device1.Id,
                EnergySourceId = es1.Id
            };

            var log2 = new ConsumptionLog
            {
                DateTime = DateTime.Now.AddMinutes(-30),
                UsedEnergy = 1.2,
                DeviceId = device2.Id,
                EnergySourceId = es1.Id
            };

            var log3 = new ConsumptionLog
            {
                DateTime = DateTime.Now.AddMinutes(-60),
                UsedEnergy = 0.8,
                DeviceId = device3.Id,
                EnergySourceId = es2.Id
            };

            context.ConsumptionLogs.AddRange(log1,log2,log3);
            context.SaveChanges();

            foreach (var log in context.ConsumptionLogs)
            {
                Console.WriteLine(
                    $"ID: {log.Id} | " +
                    $"Date: {log.DateTime} | " +
                    $"Device: {log.Device.Name} | " +
                    $"Energy Source: {log.EnergySource.Type} | " +
                    $"Used Energy: {log.UsedEnergy} kWh"
                );
            }
        }
    }
}
