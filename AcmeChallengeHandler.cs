using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Logging;

namespace LetsEncrypt.Owin
{
    public class AcmeChallengeHandler
    {
        private IFileSystem fs;
        private ILogger logger;

        public AcmeChallengeHandler(ILogger logger, string baseFolder)
        {
            this.logger = logger;
            try
            {
                fs = new PhysicalFileSystem(baseFolder);
            }
            catch (Exception e)
            {
                if (logger != null && logger.IsEnabled(System.Diagnostics.TraceEventType.Critical))
                    logger.WriteCritical("Init fail, all requests will be responsed with 404", e);
            }
        }


        public Task Handle(IOwinContext context)
        {
            if (fs == null)
            {
                logger.WriteWarning("Filesystem is not initialized properly, 404 returned");
                context.Response.StatusCode = 404;
                return Task.FromResult(404);
            }

            var filename = context.Request.Uri.Segments[context.Request.Uri.Segments.Length - 1];

            if (logger.IsEnabled(System.Diagnostics.TraceEventType.Verbose))
                logger?.WriteVerbose("Looking for file " + filename);

            IFileInfo file;
            if (!fs.TryGetFileInfo(filename, out file))
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
