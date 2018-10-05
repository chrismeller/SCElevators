using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SCElevators.Client;
using SCElevators.Data;
using SCElevators.Domain;

namespace SCElevators.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Run().ConfigureAwait(false).GetAwaiter().GetResult(); ;
        }

        private static async Task Run()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .Build();
            
            var connectionString =
                config.GetConnectionString("SCElevators.Data.ApplicationDbContext");

            using (var db = new ApplicationDbContext(connectionString))
            {
                var scraper = new ElevatorsScraper();
                var service = new ElevatorService(db);

                var next = 1;
                var errors = 0;
                var threshold = 5;
                var keepGoing = true;

                do
                {
                    try
                    {
                        Console.Write("Getting {0}", next);

                        var elevator = await scraper.GetElevator(next);

                        Console.Write(" {0}", elevator.Number);

                        if (await service.Exists(elevator.Number))
                        {
                            try
                            {
                                await service.Update(elevator.Number, DateTimeOffset.UtcNow);

                                Console.WriteLine(" Updated!");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" Unable to update!");
                            }
                        }
                        else
                        {
                            try
                            {
                                await service.Create(elevator.Number, elevator.OwnerName, elevator.Location,
                                    elevator.LocationAddress, elevator.LocationCity, elevator.LocationState,
                                    elevator.LocationZip, elevator.County, elevator.Status, elevator.NextInspectionDue,
                                    elevator.NextInspectionDueOther, elevator.MachineType, elevator.Manufacturer,
                                    elevator.UnitType, DateTimeOffset.UtcNow);

                                Console.WriteLine(" Created!");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(" Unable to create!");
                            }
                        }

                        next++;
                        errors = 0;

                        Thread.Sleep(250);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" Not found?");

                        next++;
                        errors++;

                        if (errors >= threshold)
                        {
                            keepGoing = false;
                        }
                    }
                } while (keepGoing);
            }
        }
    }
}
