using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Logging;
using PgBackup.Enums;
using PgBackup.Exceptions;
namespace PgBackup.Services
{
    public interface IPgDumpService
    {
        void BackupDB(string dbName, string backupDir, BackupFileFormat format = BackupFileFormat.Plain);
        byte[] BackupDB(string dbName, BackupFileFormat format = BackupFileFormat.Plain);

    }
    internal class PgDumpService : IPgDumpService
    {
        private readonly ILogger<PgDumpService> _logger;
        public PgDumpService(ILogger<PgDumpService> logger)
        {
            _logger = logger;
        }
        public void BackupDB(string dbName, string backupDir, BackupFileFormat format = BackupFileFormat.Plain)
        {
            _logger.LogInformation("(PgDumpService)Entered In BackupDB Method.");
            var toolFilepath = PgCommonService.GetToolFilePath("pg_dump");
            var (fileExt, cliOpt) = PgCommonService.GetFileExtension(format);
            string filename = Path.Combine(backupDir, Guid.NewGuid().ToString() + fileExt);
            string args = $"-{cliOpt} --blobs -d {dbName} --no-password -f \"{filename.Trim()}\"";
            try
            {
                _logger.LogInformation("PgDumpService Executing CLI command: {0} {1}", toolFilepath, args);
                Process process = Process.Start(new ProcessStartInfo(toolFilepath, args)
                {
                    WindowStyle = ProcessWindowStyle.Maximized,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    WorkingDirectory = PgCommonService._directory,
                    RedirectStandardError = false,
                    RedirectStandardOutput = false

                });
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new InvalidInputParameterException(ex.Message, args, ex);
            }
        }

        public byte[] BackupDB(string dbName, BackupFileFormat format = BackupFileFormat.Plain)
        {
            _logger.LogInformation("(PgDumpService)Entered In BackupDB Method.");
            var toolFilepath = PgCommonService.GetToolFilePath("pg_dump");
            var (fileExt, cliOpt) = PgCommonService.GetFileExtension(format);
            string filename = Path.Combine(PgCommonService._directory, Guid.NewGuid().ToString() + fileExt);
            string args = $"-{cliOpt} --blobs -d {dbName} --no-password -f \"{filename}\"";

            _logger.LogInformation("PgDumpService Executing CLI command: {0} {1}", toolFilepath, args);
            Process process = Process.Start(new ProcessStartInfo(toolFilepath, args)
            {
                WindowStyle = ProcessWindowStyle.Maximized,
                CreateNoWindow = false,
                UseShellExecute = false,
                WorkingDirectory = PgCommonService._directory,
                RedirectStandardError = false,
                RedirectStandardOutput = false

            });
            process.WaitForExit();
            if (File.Exists(filename))
            {
                var bytes = File.ReadAllBytes(filename);
                File.Delete(filename);
                return bytes;
            }
            throw new InvalidInputParameterException($"An error occurs excuting the comand {toolFilepath}", args);
        }
    }
}