using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Owin;

namespace LetsEncrypt.Owin
{
    public static class LetsencryptExtension
    {
        public static IAppBuilder UseLetsencrypt(this IAppBuilder app)
        {
            app.Map("/.well-known", branch =>
            {
                branch.Use((context, next) =>
                {
                    IFileInfo file;

                    var fileSystem = new PhysicalFileSystem(@".\.well-known");

                    if (!fileSystem.TryGetFileInfo(context.Request.Path.Value, out file))
                    {
                        return next();
                    }

                    return context.Response.SendFileAsync(file.PhysicalPath);
                });
            });

            return app;
        }
    }
}
