// Print from Zebra Printer via USB
// Windows Console / .NET FrameWork
// Zebra Printer SDK 2.16.2905
// Microsoft Visual Studio Community 2022 (64 ビット) - Version 17.12.1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Variables

            String formatName2 = "E:FORMAT1.ZPL";
            String zplData1 = "^XA^FO50,50^ADN,36,20^FDHello, Zebra Printer!^FS^XZ"; // Sample ZPL command
            String zplData2 = @"
                ^XA
                ^DFE:FORMAT1.ZPL
                ^FS
                ^FT26,243^A0N,56,55^FH\^FN11""First Name""^FS
                ^FT26,296^A0N,56,55^FH\^FN12""Last Name""^FS
                ^FT258,73^A0N,39,38^FH\^FDVisitor^FS
                ^BY2,4^FT403,376^B7N,4,0,2,2,N^FH^FDSerial Number^FS
                ^FO5,17^GB601,379,8^FS
                ^XZ            
                ";
            Dictionary<int, String> vars = new Dictionary<int, String>
        {
                { 12, "John" },
                { 11, "Smith" }
        };



            foreach (DiscoveredUsbPrinter printer in getUsbPrinterList())
            {
                Console.WriteLine(printer);
            }

            Console.WriteLine();


            foreach (DiscoveredPrinterDriver printer in getUsbDriverList())
            {
                Console.WriteLine(printer);
            }

            try
            {
                if (getUsbPrinterList() == null ) 
                {
                    Console.WriteLine("No Zebra USB printer found.");
                return;
                }


                // Discover USB printer
                DiscoveredUsbPrinter discoveredUsbPrinter = getUsbPrinterList().FirstOrDefault();
                if (discoveredUsbPrinter == null)
                {
                    Console.WriteLine("No Zebra USB printer found.");
                    return;
                }

                // Connect to printer
                Connection connection = discoveredUsbPrinter.GetConnection();
                connection.Open();

                // Initialize printer
                ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);

                //Send ZPL
                connection.Write(Encoding.UTF8.GetBytes(zplData2));

                // Send variables
                ZebraPrinter myprinter = ZebraPrinterFactory.GetInstance(connection);
                myprinter.PrintStoredFormat(formatName2, vars);

                // Close connection
                connection.Close();
            }
            catch (ConnectionException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }



        }

        // get connected usb printer 
        static List<DiscoveredUsbPrinter> getUsbPrinterList()
        {
            Console.WriteLine("Get connected usb printer");

            List<DiscoveredUsbPrinter> p = null;

            try
            {
                p = UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter());
            }
            catch (ConnectionException e)
            {
                Console.WriteLine($"Error discovering local printers: {e.Message}");
            }
            return p;
        }


        // get usb printer driver list 
        static List<DiscoveredPrinterDriver> getUsbDriverList()
        {
            Console.WriteLine("Get usb printer driver list.");


            List<DiscoveredPrinterDriver> p = null;

            try
            {
                p = UsbDiscoverer.GetZebraDriverPrinters();
            }
            catch (ConnectionException e)
            {
                Console.WriteLine($"Error discovering local printers: {e.Message}");
            }
            return p;
        }




        // Write usb printer list to console
        static void consoleUsbPrinterList()
        {

            // Console usb connected printers 
            try
            {
                Console.WriteLine("Get printer drivers.");
                foreach (DiscoveredPrinterDriver printer in UsbDiscoverer.GetZebraDriverPrinters())
                {
                    Console.WriteLine(printer);
                }

                Console.WriteLine();
                Console.WriteLine("Get usb connected printers.");


                foreach (DiscoveredUsbPrinter usbPrinter in UsbDiscoverer.GetZebraUsbPrinters(new ZebraPrinterFilter()))
                {
                    Console.WriteLine(usbPrinter);
                }
            }
            catch (ConnectionException e)
            {
                Console.WriteLine("Get USB connected printers");
                Console.WriteLine($"Error discovering local printers: {e.Message}");
            }

            Console.WriteLine("Done discovering local printers.");

        }
    }

}
