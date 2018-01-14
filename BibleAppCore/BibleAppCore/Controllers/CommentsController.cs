using BibleAppCore.DataLayer.Repository;
using BibleAppCore.DataLayer.TransferObjects;
using BibliaApp;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BibleAppCore.Contracts.Contract.ViewModel;
using BibleAppCore.Utilities.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;

namespace BibleAppCore.Controllers
{
    [Route("api/Comments")]
    public class CommentsController : BaseController
    {
        private IRepository Repository { get; set; }

        public CommentsController(IRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("GetComments")]
        public async Task<List<Comment>> GetAllComments()
        {

            List<Comment> comments = await Repository.GetAllComments(GetUserGuid());

            comments.ForEach(x => x.ManageCommentKeyGuid = Guid.Empty);

            return comments;

        }

        [HttpPost("AddComment"), Authorize]
        public async Task<BookExtended> AddComment([FromBody]Comment comment)
        {
            
            string login = User.Claims.FirstOrDefault(x => x.Type == "login")?.Value;
            comment.UserGuid = GetUserGuid() ?? Guid.Empty;
            comment.UserLogin = login;
            
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

            RepositoryResponse<BookExtended> repositoryResponse = await Repository.AddComment(comment);
            if (repositoryResponse.Successful)
                return repositoryResponse.Value;

            throw repositoryResponse.Exception;
        }

        [HttpPost("DeleteComment"), Authorize]
        public async Task<StatusCodeResult> DeleteComment([FromBody]Comment comment)
        {
            RepositoryResponse<Comment> repositoryResponse = await Repository.DeleteComment(comment);
            if (repositoryResponse.RepositoryResponseMessage == RepositoryResponse<Comment>.RepositoryResponseMessageEnum.NotFound)
                return new NotFoundResult();
            else if (!repositoryResponse.Successful)
                return new BadRequestResult();
            return new OkResult();
        }

    }
}
