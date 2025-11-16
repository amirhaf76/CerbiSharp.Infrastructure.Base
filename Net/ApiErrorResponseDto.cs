using CerbiSharp.Infrastructure.Base.Tools;

namespace CerbiSharp.Infrastructure.Base.Net
{
    public class ApiErrorResponseDto : ICloneable
    {
        public object Data { get; set; }

        public string Message { get; set; }

        public int? Code { get; set; }

        public object Clone()
        {
            return new ApiErrorResponseDto
            {
                Data = Data,
                Message = Message,
                Code = Code,
            };
        }

        public override string ToString()
        {
            return Helper.GetPropertyValuesInString(this);
        }
    }

}
