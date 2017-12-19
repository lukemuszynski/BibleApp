using BibleAppCore.DataLayer.Repository;
using BibleAppCore.DataLayer.TransferObjects;
using BibliaApp;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibleAppCore.Controllers
{
    [Route("api/Comments")]
    public class CommentsController : Controller
    {
        private IRepository Repository { get; set; }

        public CommentsController(IRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("GetComments")]
        public async Task<List<CommentDomainObject>> GetAllComments()
        {

            List<CommentDomainObject> comments = await Repository.GetAllComments();

            comments.ForEach(x => x.ManageCommentKeyGuid = Guid.Empty);

            return comments;

        }

        [HttpPost("AddComment")]
        public async Task<BookExtendedDomainObject> AddComment([FromBody]CommentDomainObject comment)
        {
            if (!string.IsNullOrEmpty(comment.Url))
            {
                string urlToLower = comment.Url.ToLower();
                if (urlToLower.Contains("www.youtube.com/watch?v="))
                {
                    if (comment.Url.Contains('&'))
                        comment.Url = comment.Url.FindInternalOf("watch?v=", "&");
                    else
                    {
                        comment.Url = comment.Url.Replace("https://", "").Replace("www.youtube.com/watch?v=", "");
                    }
                    comment.IsYoutubeVideo = true;
                    comment.IsAudioFile = false;
                }
                else if (urlToLower.EndsWith(".mp3") || urlToLower.EndsWith(".wav") || urlToLower.Contains("soundcloud.com"))
                {
                    comment.IsYoutubeVideo = false;
                    comment.IsAudioFile = true;
                }
                else
                {
                    comment.IsYoutubeVideo = false;
                    comment.IsAudioFile = false;
                }
            }
            else
            {
                comment.IsYoutubeVideo = false;
                comment.IsAudioFile = false;
            }

            RepositoryResponse<BookExtendedDomainObject> repositoryResponse = await Repository.AddComment(comment);
            if (repositoryResponse.Successful)
                return repositoryResponse.Value;

            throw repositoryResponse.Exception;
        }

        [HttpPost("DeleteComment")]
        public async Task<StatusCodeResult> DeleteComment([FromBody]CommentDomainObject comment)
        {
            RepositoryResponse<CommentDomainObject> repositoryResponse = await Repository.DeleteComment(comment);
            if (repositoryResponse.RepositoryResponseMessage == RepositoryResponse<CommentDomainObject>.RepositoryResponseMessageEnum.NotFound)
                return new NotFoundResult();
            else if (!repositoryResponse.Successful)
                return new BadRequestResult();
            return new OkResult();
        }

    }
}
