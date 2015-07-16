using System.Collections.Generic;
using System.Linq;

namespace WaitForSQL
{
    public class ArgParser
    {
        private readonly string[] _argStrings;

        public ArgParser(string[] argStrings)
        {
            _argStrings = argStrings;
        }

        public List<Arg> Get()
        {
            var args = new List<Arg>();


            foreach (var arg in _argStrings)
            {
                if (arg.Length > 0 && (arg[0] == '-' || arg[0] == '/'))
                {
                    args.Add(new Arg
                    {
                        Name = arg.Substring(1).ToUpper()
                    });

                    continue;
                }

                var last = args.Last();

                if (last != null)
                    last.Value = arg;
            }

            return args;
        }
    }
}