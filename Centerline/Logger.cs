using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centerline
{
    public class Logger
    {
        private static string LastLog;
        public static void LogStart(string log)
        {
            LastLog = log;
            Console.WriteLine("Starting " +log);
        }

        public static void LogEndSuccess()
        {
            if (LastLog != null)
                Console.WriteLine(LastLog + " Successfully");
            else
                Console.WriteLine("No log saved");
        }

        public static void LogEndError()
        {
            if (LastLog != null)
                Console.WriteLine("Error in " + LastLog);
            else
                Console.WriteLine("No log saved");
        }
    }
}
