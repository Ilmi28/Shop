namespace Shop.Services
{
    public class UrlService
    {
        public IHttpContextAccessor _httpContext;
        public UrlService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public string GetPreviousUrl()
        {
            var urlHost = _httpContext.HttpContext.Request.Host;
            var previousAction = _httpContext.HttpContext.Request.Cookies["previousUrl"];
            var url = "https://" + urlHost + previousAction;
            return url;
        }
        public void SetPreviousUrl()
        {
            _httpContext.HttpContext.Response.Cookies.Append("previousUrl", _httpContext.HttpContext.Request.Path);
        }
    }
}
