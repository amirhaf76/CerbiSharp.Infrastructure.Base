using CerbiSharp.Infrastructure.Base.Generator;
using CerbiSharp.Infrastructure.Base.Tools;

namespace CerbiSharp.Infrastructure.Base.UsefulDtos
{
    public class WebUserAccountDto : IWebUserAccountDto, ICloneable
    {
        private readonly string _username;
        private readonly string _firstName;
        private readonly string _lastName;

        private string _password;
        private string _nationalCode;
        private string _email;
        private string _jwt;

        private Guid _userId;
        private Guid _deviceId;


        public WebUserAccountDto() : this(
            InformationGenerator.GenerateMobileNumber(),
            InformationGenerator.GenerateSimplePassword('a'),
            InformationGenerator.GenerateFirstName(),
            InformationGenerator.GenerateSecondName(),
            InformationGenerator.GenerageNationalId(),
            string.Empty,
            string.Empty,
            Guid.Empty,
            Guid.Empty
            )
        {

        }

        public WebUserAccountDto(string username, string password) : this(
            username,
            password,
            InformationGenerator.GenerageNationalId(),
            InformationGenerator.GenerateFirstName(),
            InformationGenerator.GenerateSecondName(),
            string.Empty,
            string.Empty,
            Guid.Empty,
            Guid.Empty
            )
        {

        }

        public WebUserAccountDto(
            string username,
            string password,
            string firstName,
            string secondName,
            string nationalCode,
            string email,
            string jwt,
            Guid userId,
            Guid deviceId)
        {
            _username = username;
            _password = password;
            _firstName = firstName;
            _lastName = secondName;
            _nationalCode = nationalCode;
            _email = email;
            _jwt = jwt;
            _userId = userId;
            _deviceId = deviceId;
        }

        public Guid UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        public Guid DeviceId
        {
            get
            {
                return _deviceId;
            }

            set
            {
                _deviceId = value;
            }
        }

        public string Username
        {
            get
            {
                return _username;
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                Helper.IfNotNullSetValue(ref _password, value, nameof(Password));
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
        }

        public string NationalCode
        {
            get
            {
                return _nationalCode;
            }

            set
            {
                Helper.IfNotNullSetValue(ref _nationalCode, value, nameof(NationalCode));
            }
        }

        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                Helper.IfNotNullSetValue(ref _email, value, nameof(Email));
            }
        }

        public string Jwt
        {
            get
            {
                return _jwt;
            }

            set
            {
                Helper.IfNotNullSetValue(ref _jwt, value, nameof(Jwt));
            }
        }

        public object Clone()
        {
            var newUserInfo = new WebUserAccountDto(
                username: _username,
                password: _password,
                firstName: _firstName,
                secondName: _lastName,
                nationalCode: _nationalCode,
                email: _email,
                jwt: _jwt,
                userId: _userId,
                deviceId: _deviceId
            );

            return newUserInfo;
        }

        public override string ToString()
        {
            return $"username: {_username}, password: {_password}, firstName: {_firstName}," +
                $" secondName: {_lastName}, nationalCode: {_nationalCode}, email: {_email}," +
                $" jwt: {_jwt}, userId: {_userId}, deviceId: {_deviceId}";
        }
    }
}
