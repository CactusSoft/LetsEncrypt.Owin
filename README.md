# LetsEncrypt.Owin
Provide access to /.well-known folder in your project in order to obtain SSL certificate from [Let's Encrypt](https://letsencrypt.org)
# Usage
* Use nuget to add dependency `Install-Package LetsEncrypt.Owin`
* Add middleware `app.UseAcmeChallenge()`
