using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
namespace PgBackup.Services
{
    public interface IPgDumpService
    {
        void BackupDB(string backupDir);

    }
    public class PgDumpService : IPgDumpService
    {
        private readonly ILogger<PgDumpService> _logger;
        private static string directory;
        private static string toolFilepath;

        public PgDumpService(ILogger<PgDumpService> logger)
        {
            _logger = logger;
        }
        public void BackupDB(string backupDir)
        {
            _logger.LogInformation("(PgDumpService)Entered In BackupDB Method.");
            GetToolFilePath("pg_dump");
            try
            {
                string filename = Path.Combine(backupDir, Guid.NewGuid().ToString() + ".tar");
                string args = $"-Ft --blobs --no-password -f \"{filename.Trim()}\"";

                _logger.LogInformation("PgDumpService Executing CLI command: {0} {1}", toolFilepath, args);
                Process process = Process.Start(new ProcessStartInfo(toolFilepath, args)
                {
                    WindowStyle = ProcessWindowStyle.Maximized,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    WorkingDirectory = directory,
                    RedirectStandardError = false,
                    RedirectStandardOutput = false

                });
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private static void GetToolFilePath(string toolName)
        {
            directory = AppContext.BaseDirectory;

            //Check on what platform we are
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                toolFilepath = Path.Combine(directory, toolName + ".exe");

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
                    toolFilepath = toolName;
                }
                else
                {
                    throw new Exception("PostgrSQL client tool pg_dump does not appear to be installed on this linux system according to which command");
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
                    toolFilepath = toolName;
                }
                else
                {
                    throw new Exception("PostgrSQL client tool pg_dump does not appear to be installed on this linux system according to which command");
                }
            }
        }
    }
}