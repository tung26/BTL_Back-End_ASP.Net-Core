using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Web.Infrastructure;

namespace YourHouse.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _repository;

        public CommentService(IRepository<Comment> repository)
        {
            _repository = repository;
        }

        public async Task AddCommentAsync(CommentDto commentDto)
        {
            var comment = new Comment()
            {
                AccountId = commentDto.AccountId,
                ArticleId = commentDto.ArticleId,
                Content = commentDto.Content,
                CreateAt = commentDto.CreateAt,
                CommentId = commentDto.CommentId,
                ParentCommentId = commentDto.ParentCommentId,
            };

            _repository.AddAsync(comment);
            await _repository.SaveChangeAsync();
        }

        public async void DeleteCommentAsync(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            _repository.DeleteAsync(comment);
            await _repository.SaveChangeAsync();
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentAsync()
        {
            var comments = await _repository.GetAllAsync();
            return comments.Select(comment => new CommentDto()
            {
                AccountId = comment.AccountId,
                CommentId = comment.CommentId,
                ArticleId = comment.ArticleId,
                Content = comment.Content,
                CreateAt = comment.CreateAt,
                ParentCommentId = comment.ParentCommentId,
            });
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            return comment == null ? null : comment;
        }

        public async void UpdateComment(Comment comment)
        {
            _repository.UpdateAsync(comment);
            await _repository.SaveChangeAsync();
        }
    }
}
