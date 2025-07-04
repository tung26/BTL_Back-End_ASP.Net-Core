using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Infrastructure;

namespace YourHouse.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _repository;

        public CommentService(IRepository<Comment> repository)
        {
            _repository = repository;
        }

        public async Task<int> AddCommentAsync(CommentDto commentDto)
        {
            var comment = new Comment()
            {
                AccountId = commentDto.AccountId,
                ArticleId = commentDto.ArticleId,
                Content = commentDto.Content,
                ParentCommentId = commentDto.ParentCommentId,
            };

            Console.WriteLine("ok here");

            await _repository.AddAsync(comment);
            await _repository.SaveChangeAsync();

            return comment.CommentId;
        }

        public async Task DeleteCommentAsync(int id, bool articleDelete = false)
        {
            var comments = await _repository.GetAllAsync();
            var comment = await _repository.GetByIdAsync(id);

            if (comment != null)
            {
                if (comment.InverseParentComment.Count() != 0 && articleDelete == false)
                {
                    comment.IsDelete = true;
                    _repository.UpdateAsync(comment);
                    await _repository.SaveChangeAsync();
                }
                else
                {
                    _repository.DeleteAsync(comment);
                    await _repository.SaveChangeAsync();
                }
            }
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
                IsDelete = comment.IsDelete,
                ParentCommentId = comment.ParentCommentId,
            });
        }

        public async Task<CommentDto?> GetCommentByIdAsync(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            return comment == null ? null : new CommentDto()
            {
                AccountId = comment.AccountId,
                CommentId = comment.CommentId,
                ArticleId = comment.ArticleId,
                Content = comment.Content,
                CreateAt = comment.CreateAt,
                IsDelete = comment.IsDelete,
                ParentCommentId = comment.ParentCommentId,
            };
        }

        public async Task UpdateComment(CommentDto commentDto)
        {
            //_repository.UpdateAsync(comment);
            await _repository.SaveChangeAsync();
        }
    }
}
