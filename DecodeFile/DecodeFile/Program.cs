using System;
using System.IO;
using System.Threading;

namespace DecodeFile
{
    class Program
    {
        static void Main(string[] args)
        {
            DecodeFile("data/qrlog", "data/filedecoded");
            Console.WriteLine("\nDecode finished! Automatically close after 5 seconds!");
            Thread.Sleep(5000);
        }

        public static void DecodeFile(String file, String path)
        {
            string line;
            string store = "";
            string check1 = ".diwf.com): query: ";
            string check2 = ".diwf.com IN A + (";
            using (StreamReader sr = File.OpenText(file))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if( line.Contains(check1) == true && line.Contains(check2) == true )
                    {
                        int begin = line.LastIndexOf(check1);
                        int end = line.IndexOf(check2);
                        line = line.Substring(  begin + 19, end - begin - 19 );
                        store = store + line;
                    }
                }
            }
            Console.Write(store);
            byte[] b = Convert.FromBase64String(store);
            File.WriteAllBytes(path, b);
        }
    }
}
