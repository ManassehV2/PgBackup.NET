using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using PgBackup.Enums;
using PgBackup.Exceptions;

namespace PgBackup.Services
{
    internal static class PgCommonService
    {
        public static string _directory;
        public static string _toolFilepath;

        public static void GetToolFilePath(string toolName)
        {
            _directory = AppContext.BaseDirectory;

            //Check on what platform we are
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                _toolFilepath = Path.Combine(_directory, toolName + ".exe");

                if (!File.Exists(_toolFilepath))
                {
                    var assembly = typeof(PgDumpService).GetTypeInfo().Assembly;
                    var type = typeof(PgDumpService);
                    var ns = type.Namespace;

                    using (var resourceStream = assembly.GetManifestResourceStream($"{ns}.{toolName}.exe"))
                    using (var fileStream = File.OpenWrite(_toolFilepath))
                    {
                        resourceStream.CopyTo(fileStream);
                    }
                }
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                //Check if wkhtmltoimage package is installed in using which command
                Process process = Process.Start(new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = "/usr/local/bin",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "which",
                    Arguments = toolName

                });
                string answer = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(answer) && answer.Contains(toolName))
                {
                    _toolFilepath = toolName;
                }
                else
                {
                    throw new DependencyPgToolsNotFoundException(toolName);
                }
            }
            else
            {
                //Check if pg_dump  is installed on this distro of MacOS using which command
                Process process = Process.Start(new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = "/usr/local/bin",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "which",
                    Arguments = toolName

                });
                string answer = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(answer) && answer.Contains("pg_dump"))
                {
                    _toolFilepath = toolName;
                }
                else
                {
                    throw new DependencyPgToolsNotFoundException(toolName);
                }
            }
        }
        public static (string, string) GetFileExtension(BackupFileFormat format)
        {
            switch (format)
            {
                case BackupFileFormat.Tar:
                    return (".tar", "Ft");
                case BackupFileFormat.Custom:
                    return (".dump", "Fc");
                default:
                    return (".sql", "Fp");
            }

        }
    }
}