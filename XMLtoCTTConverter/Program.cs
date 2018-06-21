using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace XMLtoCTTConverter
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter full path name to read XML instance:");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Example: C:/Users/QHyseni/instance.xml ");
            Console.ResetColor();
            var xmlPathname = Console.ReadLine();

            Console.WriteLine("Enter full path name to write ECTT instance:");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Example: C:/Users/QHyseni/newinstance.ectt ");
            Console.ResetColor();
            var ecttPathname = Console.ReadLine();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPathname);

            try
            {
                XMLtoECTTLib.Converter.ConvertToECTT(xmlDoc, ecttPathname);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Exception: " + ex.Message);
                Console.ResetColor();
            }
            
        }
    }
}
