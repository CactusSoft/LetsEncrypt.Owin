using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Logging;

namespace LetsEncrypt.Owin
{
    public class AcmeChallengeHandler
    {
        private Lazy<IFileSystem> filesystem;
        private ILogger logger;

        public AcmeChallengeHandler(ILogger logger, string baseFolder)
        {
            this.logger = logger;
            filesystem = new Lazy<IFileSystem>(() => new PhysicalFileSystem(baseFolder));           
        }


        public Task Handle(IOwinContext context)
        {      
            var filename = context.Request.Uri.Segments[context.Request.Uri.Segments.Length - 1];
            if (logger.IsEnabled(System.Diagnostics.TraceEventType.Verbose))
                logger?.WriteVerbose("Looking for file " + filename);

            IFileInfo file;
            if (!filesystem.Value.TryGetFileInfo(filename, out file))
            {
                logger.WriteWarning("File not found, returns 404");
                context.Response.StatusCode = 404;
                return Task.FromResult(404);
            }

            if (logger.IsEnabled(System.Diagnostics.TraceEventType.Verbose))
                logger.WriteVerbose("Return file " + file.PhysicalPath);

            return context.Response.SendFileAsync(file.PhysicalPath);
        }
    }
}
