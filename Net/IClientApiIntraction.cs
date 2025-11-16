namespace CerbiSharp.Infrastructure.Base.Net
{
    public interface IClientApiIntraction
    {
        string ClientId { get; set; }
        Guid DeviceId { get; set; }
        string DomainUrl { get; set; }
        string JwtToken { get; set; }

        public HttpClient CreateHttpClient();
    }
}