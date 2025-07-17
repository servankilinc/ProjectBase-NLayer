using System.Text;

namespace WebUI.Utils.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool IsJsonRequest(this HttpContext httpContext)
        {
            return httpContext.Request.Headers["Accept"].ToString().Contains("application/json") ||
                httpContext.Request.Headers["X-Requested-With"].ToString().Contains("XMLHttpRequest") ||
                httpContext.Request.ContentType?.Contains("application/json") == true;
        }

        public static string GetUrl(this HttpContext httpContext)
        {
            string SchemeDelimiter = Uri.SchemeDelimiter;
            var scheme = httpContext.Request.Scheme ?? string.Empty;
            var host = httpContext.Request.Host.Value ?? string.Empty;
            var pathBase = httpContext.Request.PathBase.Value ?? string.Empty;
            var path = httpContext.Request.Path.Value ?? string.Empty;
            var queryString = httpContext.Request.QueryString.Value ?? string.Empty;

            var length = scheme.Length + SchemeDelimiter.Length + host.Length + pathBase.Length + path.Length + queryString.Length;

            return new StringBuilder(length)
                .Append(scheme)
                .Append(SchemeDelimiter)
                .Append(host)
                .Append(pathBase)
                .Append(path)
                .Append(queryString)
                .ToString();
        }

		public static string GetPath(this HttpContext httpContext)
		{
			var path = httpContext.Request.Path.Value ?? string.Empty;

			return path.Trim();
		}

        public static string GetBasePath(this HttpContext httpContext)
        {
			string? controller = httpContext.GetRouteData().Values["controller"]?.ToString()?.Trim();
			string? action = httpContext.GetRouteData().Values["action"]?.ToString()?.Trim();

			return $"/{controller}/{action}";
		}


		public static string GetLightMode(this HttpContext httpContext)
        { 
            return httpContext.Request.Cookies["light_mode"] ?? "system";
        }

        public static void SetLightMode(this HttpContext httpContext, string mode)
        {
            httpContext.Response.Cookies.Append("light_mode", mode, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
            });
        }
    }
}
