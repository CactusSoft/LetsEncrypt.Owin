using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Owin;

namespace LetsEncrypt.Owin
{
    public static class LetsencryptExtension
    {
        public static readonly string wellKnownFolderName = ".well-known";
        public static readonly string acmeChallangeFolderName = "acme-challenge";

        public static IAppBuilder UseAcmeChallenge(this IAppBuilder app)
        {
            var acmePathSegment = new PathString('/' + wellKnownFolderName + '/' + acmeChallangeFolderName);
            var handler = new AcmeChallengeHandler(app.CreateLogger(typeof(AcmeChallengeHandler)), '.' + acmePathSegment.Value);

            app.MapWhen((ctx) => ctx.Request.Path.StartsWithSegments(acmePathSegment), branch => branch.Run(handler.Handle));

            return app;
        }
    }
}
