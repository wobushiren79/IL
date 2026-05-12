using System;
using System.Collections.Generic;
public partial class TextTalkBean
{
    public TextTalkBean()
    {

    }
    public TextTalkBean(TextInfoTypeEnum type, int selectType, string content)
    {
        this.type = (int)type;
        text_order = 1;
        this.select_type = selectType;
        this.content_language = content;
    }

    public TextInfoTypeEnum GetTextType()
    {
        return (TextInfoTypeEnum)type;
    }

    public TextTalkTypeEnum GetTextTalkType()
    {
        return (TextTalkTypeEnum)talk_type;
    }
}
public partial class TextTalkCfg
{
}
