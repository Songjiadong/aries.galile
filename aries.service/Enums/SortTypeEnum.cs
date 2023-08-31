using System.ComponentModel;

namespace aries.service.galileo
{
    public enum SortTypeEnum
    {
        [Description("相关性")]
        Score=0,
        [Description("发布时间")]
        PublishTime=1,
    }
}
