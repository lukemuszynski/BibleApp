using System;

namespace BibliaApp
{
    
        public class CommentDomainObject
        {
            public Guid Guid { get; set; }
            public string Url { get; set; }
            public string Title { get; set; }
            public bool IsYoutubeVideo { get; set; }
            public bool IsAudioFile { get; set; }
            public string Text { get; set; }
            public string StartIndex { get; set; }
            public string EndIndex { get; set; }
            public Guid BookGuid { get; set; }
            public DateTime AddTime { get; set; }
        }
    
}