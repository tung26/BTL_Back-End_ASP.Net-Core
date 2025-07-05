using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourHouse.Application.DTOs;
using YourHouse.Application.Interfaces;
using YourHouse.Domain.Interfaces;
using YourHouse.Infrastructure;

namespace YourHouse.Application.Services
{
    public class AccountService : IAccountService
    {

        private readonly IRepository<Account> _repository;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly ILikeArticleService _likeArticleService;

        public AccountService(IRepository<Account> repository, 
            ICommentService commentService,
            IArticleService articleService,
            ILikeArticleService likeArticleService 
            )
        {
            _repository = repository;
            _commentService = commentService;
            _articleService = articleService;
            _likeArticleService = likeArticleService;
        }

        public async Task AddAccountAsync(AccountDto accountDto)
        {
            try
            {
                var account = new Account()
                {
                    FullName = accountDto.FullName,
                    PasswordHash = accountDto.PasswordHash,
                    Email = accountDto.Email,
                    Phone = accountDto.Phone,
                    RoleId = accountDto.RoleId
                };

                await _repository.AddAsync(account);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task DeleteAccountAsync(int id)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);
                var articles = await _articleService.GetAllArticleAsync();
                var comments = await _commentService.GetAllCommentAsync();
                var likeArticles = await _likeArticleService.GetAllLikeArticleAsync();

                if (likeArticles != null)
                {
                    foreach (var likeArticle in likeArticles)
                    {
                        if (likeArticle == null) continue;
                        if (likeArticle.AccountId == account.AccountId)
                        {
                            await _likeArticleService.DeleteLikeArticleAsync(likeArticle.LikeArticleId);
                        }
                    }
                }

                if (comments != null)
                {
                    foreach (var comment in comments)
                    {
                        if (comment == null) continue;
                        if (comment.AccountId == account.AccountId)
                        {
                            await _commentService.DeleteCommentAsync(comment.CommentId, true);
                        }
                    }
                }

                if (articles != null)
                {
                    foreach (var article in articles)
                    {
                        if (article == null) continue;

                        if (article.AccountId == account.AccountId)
                        {
                            foreach (var comment in comments)
                            {
                                if (comment == null) continue;
                                if (comment.ArticleId == article.ArticleId)
                                {
                                    await _commentService.DeleteCommentAsync(comment.CommentId, true);
                                }
                            }
                            await _articleService.DeleteArticleAsync(article.ArticleId);
                        }
                    }
                }
                if (account != null)
                {
                    _repository.DeleteAsync(account);
                    await _repository.SaveChangeAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            try
            {
                var account = await _repository.GetByIdAsync(id);

                return account == null ? null : new AccountDto()
                {
                    AccountId = account.AccountId,
                    FullName = account.FullName,
                    PasswordHash = account.PasswordHash,
                    Email = account.Email,
                    Phone = account.Phone,
                    RoleId = account.RoleId,
                    ImageUser = account.ImageUser,
                    Facebook = account.Facebook,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountAsync()
        {
            try
            {
                var accounts = await _repository.GetAllAsync();
                return accounts.Select(account => new AccountDto()
                {
                    AccountId = account.AccountId,
                    FullName = account.FullName,
                    PasswordHash = account.PasswordHash,
                    Email = account.Email,
                    Phone = account.Phone,
                    RoleId = account.RoleId,
                    ImageUser = account.ImageUser,
                    Facebook = account.Facebook,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAccount(AccountDto accountDto)
        {
            try
            {
                var account = await _repository.GetByIdAsync(accountDto.AccountId);

                account.FullName = accountDto.FullName;
                account.PasswordHash = accountDto.PasswordHash;
                account.Email = accountDto.Email;
                account.Phone = accountDto.Phone;
                account.RoleId = accountDto.RoleId;
                account.ImageUser = accountDto.ImageUser;
                account.Facebook = accountDto.Facebook;

                _repository.UpdateAsync(account);
                await _repository.SaveChangeAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountDto?> GetAccountByEmailAsync(string email)
        {
            try
            {
                var accounts = await _repository.GetAllAsync();
                var acc = accounts.Where(b => b.Email == email).FirstOrDefault();
                return acc == null ? null : new AccountDto()
                {
                    AccountId = acc.AccountId,
                    FullName = acc.FullName,
                    PasswordHash = acc.PasswordHash,
                    Email = acc.Email,
                    Phone = acc.Phone,
                    RoleId = acc.RoleId,
                    ImageUser = acc.ImageUser,
                    Facebook = acc.Facebook,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsValidAccount(string email, string password)
        {
            try
            {
                var account = await _repository.GetAllAsync();
                bool isSuccess = account.Any(b => b.Email == email && b.PasswordHash == password);
                return isSuccess;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
