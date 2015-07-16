using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using WaitForSql;
using WaitForSQL;

namespace WaitForSql.UnitTests
{
    [TestFixture]
    public class ArgumensParserTests
    {

        [Test]
        public void finds_named_arg()
        {
            const string serverName = "blah,1234";
            const string argName = "-S";

            var parser = new ArgParser(new []{argName, serverName});
            var args = parser.Get();

            Assert.AreEqual(1, args.Count);
            Assert.AreEqual('S', args[0].Name[0]);
            Assert.AreEqual(serverName, args[0].Value);
            
        }

        [Test]
        public void arg_name_is_captialized()
        {
            const string argName = "/s";

            var parser = new ArgParser(new[] { argName });
            var args = parser.Get();

            Assert.AreEqual(1, args.Count);
            Assert.AreEqual('S', args[0].Name[0]);
            

        }


        [Test]
        public void finds_arg_with_no_value()
        {
            const string argName = "-E";

            var parser = new ArgParser(new[] { argName });
            var args = parser.Get();

            Assert.AreEqual(1, args.Count);
            Assert.AreEqual('E', args[0].Name[0]);
            
        }

    }
}
