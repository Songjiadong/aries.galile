namespace aries.galileo.query
{
    public class SearchItemInfo
    {
        public SearchItemInfo() { }
        /// <summary>
        /// Id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string? Url { get; set; }
        /// <summary>
        /// 数据来源
        /// </summary>
        public string? Source { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string? PublishedAt { get; set; }
    }
}
