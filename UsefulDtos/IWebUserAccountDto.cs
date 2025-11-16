namespace CerbiSharp.Infrastructure.Base.UsefulDtos
{
    public interface IWebUserAccountDto : ICloneable
    {
        public Guid UserId { get; set; }

        public Guid DeviceId { get; set; }

        public string Username { get; }

        public string Password { get; set; }

        public string FirstName { get; }

        public string LastName { get; }

        public string NationalCode { get; set; }

        public string Email { get; set; }

        public string Jwt { get; set; }
    }
}
