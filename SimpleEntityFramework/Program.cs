using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SimpleEntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

            using (var db = new SecurityContext())
            {
                Console.Write("Enter a security: ");
                var level = Console.ReadLine();

                Security security = new Security() {Symbol = level};

                db.Securities.Add(security);
                db.SaveChanges();

                Console.WriteLine("New ID = {0}", security.Id);

                DailyStat dailyStat = new DailyStat()
                    {
                        Date = DateTime.Now,
                        Close = 10,
                        Open = 9,
                        Security = security
                    };
                DailyStat dailyStat2 = new DailyStat()
                    {
                        Date = DateTime.Now,
                        Close = 11,
                        Open = 10,
                        Security = security
                    };

                db.DailyStats.Add(dailyStat);
                db.DailyStats.Add(dailyStat2);
                db.SaveChanges();

                Security newSecurity = db.Securities.FirstOrDefault(s => s.Id == security.Id);
                DailyStat newStat = db.DailyStats.FirstOrDefault(s => s.Id == dailyStat.Id);

                Console.ReadKey();
            }
        }
    }

    public class Security
    {
        public int Id { get; set; }
        public string Symbol { get; set; }

        public virtual List<DailyStat> DailyStats { get; set; }
    }

    public class DailyStat
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal Close { get; set; }

        public virtual Security Security { get; set; }
    }

    public class SecurityContext : DbContext
    {
        public DbSet<Security> Securities { get; set; }
        public DbSet<DailyStat> DailyStats { get; set; }
    }
}

