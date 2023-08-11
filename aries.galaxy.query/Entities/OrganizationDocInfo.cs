using aries.common;


namespace aries.galaxy.query
{
    public class OrganizationDocInfo
    {

        /// <summary>
        /// 标题
        /// </summary>
       // [Text(Analyzer = "ik_max_word", Index = true)]
        public string? Name { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
       // [Date]
        public DateTime PublishedAt { get; set; }
        /// <summary>
        /// Url
        /// </summary>
        //[Text]
        public string? Url { get; set; }
        /// <summary>
        /// Logo
        /// </summary>
        //[Text]
        public string? Logo { get; set; }


    }


}
