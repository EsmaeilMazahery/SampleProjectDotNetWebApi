using System;
using System.Linq;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SP.ServiceLayer.Services
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Member Register(string name, string family, string mobile, string email, string password);

        bool checkEmailTaken(string email);
        bool checkMobileTaken(string mobile);
        Member Read(int userId);
        IQueryable<Member> Read(string username);
        IQueryable<Member> ReadByEmail(string email);
        void VerifyMobile(int userId);
        void VerifyEmail(int userId);

        void Enable(int userId, bool Enable);

        void changeImage(int userId,string address);
    }

    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Member>> _users;
        public MemberRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _users = new Lazy<DbSet<Member>>(() => _uow.Set<Member>());
        }

        public IQueryable<Member> Read(string username)
        {
            return _users.Value.Where(w => w.mobile == username || w.email==username);
        }

        public IQueryable<Member> ReadByEmail(string email)
        {
            return _users.Value.Where(w => w.email == email);
        }

        public Member Register(string name, string family, string mobile, string email, string password)
        {
            try
            {
                Member user = new Member()
                {
                    name = name,
                    family = family,
                    mobile = mobile,
                    password = IdentityCryptography.HashPassword(password),
                    email = email,
                    enable = true,
                    verifyMobile = false
                };
                var euser = _uow.AddEntity(user);

                return euser.Entity;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        
        public void VerifyMobile(int userId)
        {
            try
            {
                using (var transaction = _uow.BeginTransaction())
                {
                    try
                    {
                        // update record
                        Update(f => f.userId  == userId, u => new Member { verifyMobile = true });

                        // save
                        _uow.Save();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public void VerifyEmail(int userId)
        {
            try
            {
                using (var transaction = _uow.BeginTransaction())
                {
                    try
                    {
                        // update record
                        Update(f => f.userId == userId, u => new Member { verifyEmail = true });

                        // save
                        _uow.Save();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        
        public void Enable(int userId, bool Enable)
        {

            try
            {
                using (var transaction = _uow.BeginTransaction())
                {
                    try
                    {
                        // update record
                        Update(f => f.userId == userId, u => new Member { enable = Enable });

                        // save
                        _uow.Save();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public bool checkEmailTaken(string email)
        {
            return _users.Value.Any(w => w.email == email && w.verifyEmail && w.complate);
        }

        public bool checkMobileTaken(string mobile)
        {
            return _users.Value.Any(w => w.mobile == mobile && w.verifyMobile && w.complate);
        }

        public Member Read(int userId)
        {
            return _users.Value.Where(w=>w.userId==userId).FirstOrDefault();
        }

         public void changeImage(int userId, string imageAddress)
        {
            try
            {
                using (var transaction = _uow.BeginTransaction())
                {
                    try
                    {
                        // update record
                        Update(f => f.userId == userId, u => new Member { image = imageAddress });

                        // save
                        _uow.Save();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
    }
}
