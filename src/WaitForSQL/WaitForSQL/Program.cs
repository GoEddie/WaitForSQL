using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace WaitForSQL
{
    internal class Program
    {
        private static int Main(string[] arguments)
        {
            var args = new ArgParser(arguments).Get();

            if (args.FirstOrDefault(p => p.Name == "S") == null)
                return PrintError("No Server name specified");

            if (args.FirstOrDefault(p => p.Name == "D") == null)
                return PrintError("No Database name specified");


            var trusted = args.FirstOrDefault(p => p.Name == "E") != null;
            var sqlAuth = args.FirstOrDefault(p => p.Name == "U") != null &&
                          args.FirstOrDefault(p => p.Name == "P") != null;

            if (!trusted && !sqlAuth)
                return PrintError("You need to either specify trusted or sql auth details");

            var timeoutSeconds = 300;
            if (args.FirstOrDefault(p => p.Name == "T") != null)
            {
                timeoutSeconds = int.Parse(args.FirstOrDefault(p => p.Name == "T").Value);
            }

            var auth = GetAuthString(trusted, args);

            var connectionString = string.Format("Server={0};Initial Catalog={1};{2};Connect Timeout=3;",
                args.FirstOrDefault(p => p.Name == "S").Value, args.FirstOrDefault(p => p.Name == "D").Value, auth);

            var timeStart = DateTime.Now;

            while (true)
            {
                if (timeStart < DateTime.Now.Subtract(new TimeSpan(0, 0, 0, timeoutSeconds)))
                {
                    Console.WriteLine("Error Timeout");
                    return (int) ExitCode.Timeout;
                }

                try
                {
                    new SqlConnection(connectionString).Open();
                    return (int) ExitCode.Success;
                }
                catch (Exception)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static string GetAuthString(bool trusted, List<Arg> args)
        {
            if (trusted)
                return "Integrated Security=SSPI;";

            return string.Format("UID={0};PWD={1};", args.First(p => p.Name == "U").Value,
                args.First(p => p.Name == "P").Value);
        }

        private static int PrintError(string message)
        {
            Console.WriteLine(
                "Error - Missing Arg: {0}\r\nTo run specify: \"-S servername,port\" and \"-D database name\" and then either -E or -U name -P pass",
                message);
            return (int) ExitCode.ArgError;
        }
    }
}