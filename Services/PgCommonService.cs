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
        public static readonly string _directory = AppContext.BaseDirectory;

        public static string GetToolFilePath(string toolName)
        {
            //Check on what platform we are
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                var toolFilepath = Path.Combine(_directory, toolName + ".exe");

                if (!File.Exists(toolFilepath))
                {
                    var assembly = typeof(PgDumpService).GetTypeInfo().Assembly;
                    var type = typeof(PgDumpService);
                    var ns = type.Namespace;

                    using (var resourceStream = assembly.GetManifestResourceStream($"{ns}.{toolName}.exe"))
                    using (var fileStream = File.OpenWrite(toolFilepath))
                    {
                        resourceStream.CopyTo(fileStream);
                    }
                }
                return toolFilepath;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                //Check if pg_dump package is installed in using which command
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
                    return toolName;
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
                    return toolName;
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