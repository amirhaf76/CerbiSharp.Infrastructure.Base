namespace CerbiSharp.Infrastructure.Base.Net
{
    public class ApiItemContainer<T>
    {
        public long TotalCount { get; private set; }

        public List<T> Items { get; private set; }

        public static ApiItemContainer<T> CreateApiItemContainer<TCorrespondItem>(
            long count,
            IEnumerable<TCorrespondItem> currespondItems,
            Func<TCorrespondItem, T> convertor
            )
        {
            var apiItemContainer = new ApiItemContainer<T>
            {
                TotalCount = count,
                Items = currespondItems.Select(convertor).ToList()
            };

            return apiItemContainer;
        }
    }
}
