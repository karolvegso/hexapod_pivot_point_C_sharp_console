using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace hexapod_pivot_point_console
{
    class Program
    {
        static void Main(string[] args)
        {
            /* this program perform fast reference of hexapod, 
             * set pivot point and print pivot point*/
            // ask for insertion of hexapod IP address
            Console.WriteLine("Insert IP address of hexapod: ");
            string server_hexapod = Console.ReadLine();
            // ask for insertion of hexpod port
            Console.WriteLine("Insert port number of hexapod (e.g. 50000): ");
            string port_hexapod_string = Console.ReadLine();
            Int32 port_hexapod = Int32.Parse(port_hexapod_string);
            // create session with hexapod
            TcpClient session_hexapod = new TcpClient(server_hexapod, port_hexapod);
            NetworkStream stream_hexapod = session_hexapod.GetStream();
            // do fast reference of hexapod
            Console.WriteLine("In order to set pivot point, as first step, hexapod must be fast referenced.");
            Console.WriteLine("Do you want to fast reference hexapod (Y/N)?");
            string response_str = Console.ReadLine();
            // declare data as byte array in method and initialize it to null
            Byte[] data = null;
            if (response_str == "Y" || response_str == "y")
            {
                // command string for fast reference of hexapod (FRF)
                Console.WriteLine("FRF");
                string cmd_frf_str = "FRF" + "\n";
                data = System.Text.Encoding.ASCII.GetBytes(cmd_frf_str);
                stream_hexapod.Write(data, 0, data.Length);
                Console.WriteLine("How long should we wait for hexapod fast referencing in seconds? ");
                string time_to_wait_sec_str = Console.ReadLine();
                Int32 time_to_wait_sec = Int32.Parse(time_to_wait_sec_str);
                Int32 time_to_wait_ms = time_to_wait_sec * 1000;
                Thread.Sleep(time_to_wait_ms);
            }
            else
            {
                Console.WriteLine("Continue to set pivot point without fast referencing.");
            }
            // set pivot point R, S, T
            // insert pivot point R - in X direction
            Console.WriteLine("Set pivot point R - in X direction in mm: ");
            string R_str = Console.ReadLine();
            // insert pivot point S - in Y direction
            Console.WriteLine("Set pivot point S - in Y direction in mm: ");
            string S_str = Console.ReadLine();
            // insert pivot point T - in Z direction
            Console.WriteLine("Set pivot point T - in Z direction in mm: ");
            string T_str = Console.ReadLine();
            // set pivot point R
            string cmd_spi_R_str = "SPI R " + R_str + "\n";
            data = System.Text.Encoding.ASCII.GetBytes(cmd_spi_R_str);
            stream_hexapod.Write(data, 0, data.Length);
            Console.WriteLine(cmd_spi_R_str);
            Thread.Sleep(250);
            data = null;
            // set pivot point S
            string cmd_spi_S_str = "SPI S " + S_str + "\n";
            data = System.Text.Encoding.ASCII.GetBytes(cmd_spi_S_str);
            stream_hexapod.Write(data, 0, data.Length);
            Console.WriteLine(cmd_spi_S_str);
            Thread.Sleep(250);
            data = null;
            // set pivot point T
            string cmd_spi_T_str = "SPI T " + T_str + "\n";
            data = System.Text.Encoding.ASCII.GetBytes(cmd_spi_T_str);
            stream_hexapod.Write(data, 0, data.Length);
            Console.WriteLine(cmd_spi_T_str);
            Thread.Sleep(250);
            data = null;
            // query for set pivot point
            string cmd_spi_qm = "SPI?" + "\n";
            data = System.Text.Encoding.ASCII.GetBytes(cmd_spi_qm);
            stream_hexapod.Write(data, 0, data.Length);
            Console.WriteLine(cmd_spi_qm);
            data = null;
            // read hexapod response
            data = new Byte[256];
            string response_spi_qm = string.Empty;
            Int32 bytes = stream_hexapod.Read(data, 0, data.Length);
            response_spi_qm = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            data = null;
            Thread.Sleep(250);
            // print response of set point query on the screen
            Console.Write(response_spi_qm);
            // close stream and session, close all
            stream_hexapod.Flush();
            stream_hexapod.Close();
            session_hexapod.Close();
            // aditional last wait to postpone console window closure
            Thread.Sleep(10000);
            // end of console program
        }
    }
}
