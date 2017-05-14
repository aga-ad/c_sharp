using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homework7
{
    class Program
    {

        static String ExecuteDisassembler(String pathToIldasm, String executable)
        {
            String code = "";
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = pathToIldasm,
                    Arguments = executable + " /text",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            
            process.Start();
            
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                code += line + "\n";
                // do something with line
            }
            return code;
        }

        static void ExecuteAssembler(String pathToIlasm, String executable, String assembly)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = pathToIlasm,
                    Arguments = assembly + " /output:" + executable,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            
        }

        

        static void Main(string[] args)
        {
            const String tempFile = "temp.il"; // any filename
            const String pathToIldasm = "\"C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v10.0A\\bin\\NETFX 4.6.1 Tools\\ildasm.exe\""; //change to your directory with ildasm.exe
            const String pathToIlasm = "\"c:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\ilasm.exe\""; // change to your directory with ilasm.exe

            const String source = "HelloWorld.exe"; // HelloWorld program to hack
            const String output = "HelloWorld_patched.exe"; // name for hacked HelloWorld
            




            String assembly = ExecuteDisassembler(pathToIldasm, source);
            assembly = assembly.Replace("Hello, World!", "Goodbye, World!");

            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
            using (FileStream fs = File.Create(tempFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(assembly);
                fs.Write(info, 0, info.Length);
            }
            ExecuteAssembler(pathToIlasm, output, tempFile);
        }
    }
}
