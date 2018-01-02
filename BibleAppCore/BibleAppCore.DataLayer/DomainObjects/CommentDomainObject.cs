using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliaApp
{
    [Table("Comment")]
    internal class CommentDomainObject
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
        public Guid ManageCommentKeyGuid { get; set; }
        public string UserLogin { get; set; }
        public Guid UserGuid { get; set; }
    }

}