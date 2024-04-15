using Common.Paging;
using ProfileDAL.ViewModels;
using Common.Repositories;
using Common.Extensions;
using API.All.DbContexts;
using API.Entities;

namespace ProfileDAL.Repositories
{
    public class VoteRepository : RepositoryBase<HU_ANSWER>, IVoteRepository
    {
        private ProfileDbContext _appContext => (ProfileDbContext)_context;
        public VoteRepository(ProfileDbContext context) : base(context)
        {

        }


        /// <summary>
        /// Thêm câu hỏ hoặc bình chọn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> AddAnswer(QuestionOutputDTO param)
        {
            var q = await _appContext.Questions.FindAsync(param.Id);
            if (q == null)
            {
                return new ResultWithError(Message.RECORD_NOT_FOUND);
            }

            if (q.EXPIRE != null && DateTime.Now > q.EXPIRE)
            {
                return new ResultWithError(Message.EXPIRE_DATE);
            }

            var m = param.Answers.Where(c => c.IsVote == true).Select(d => d.Id).ToList();
            var answerIds = param.Answers.Select(d => d.Id).ToList();
            var m2 = (from p in _appContext.Answers
                      join u in _appContext.AnswerUsers on p.ID equals u.ANSWER_ID
                      join qs in _appContext.Questions on p.QUESTION_ID equals qs.ID
                      where !answerIds.Contains(p.ID) && u.EMP_ID == _appContext.EmpId && qs.ID == param.Id
                      select p).ToList();

            if (q.IS_MULTIPLE == false && (m.Count() + m2.Count()) > 1)
            {
                return new ResultWithError(Message.NOT_MULTIPLE);
            }
            var n = param.Answers.Where(c => c.Id == 0).Count();
            if (q.IS_ADD_ANSWER == false && n > 0)
            {
                return new ResultWithError(Message.NOT_ADD_ANSWER);
            }


            if (param.Answers != null && param.Answers.Count > 0)
            {

                foreach (var item in param.Answers)
                {
                    if (item.Id == null) // them moi cau hoi
                    {
                        var aswerNew = Map(item, new HU_ANSWER());
                        aswerNew.QUESTION_ID = q.ID;

                        _appContext.Answers.Add(aswerNew);

                        if (item.IsVote == true)
                        {
                            var answerUser = new HU_ANSWER_USER();
                            answerUser.EMP_ID = _appContext.EmpId;
                            answerUser.ANSWER_ID = aswerNew.ID;
                            _appContext.AnswerUsers.Add(answerUser);
                        }

                    }
                    else  // vote nhung cau tra loi cu
                    {
                        var answerEdit = await _appContext.Answers.FindAsync(item.Id);
                        if (answerEdit == null)
                        {
                            return new ResultWithError("ANSWER_NOT_FOUND");
                        }
                        var r = _appContext.AnswerUsers.Where(c => c.ANSWER_ID == item.Id && c.EMP_ID == _appContext.EmpId).FirstOrDefault();
                        if (item.IsVote == true && r == null)
                        {
                            _appContext.AnswerUsers.Add(new HU_ANSWER_USER() { EMP_ID = _appContext.EmpId, ANSWER_ID = (int)item.Id });
                        }
                        if (item.IsVote == false && r != null)
                        {
                            _appContext.AnswerUsers.Remove(r);
                        }

                    }
                }
            }

            try
            {
                _appContext.SaveChanges();
                return new ResultWithError(200);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// Get Câu hỏi mới nhất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetQuestion(int? id)
        {
            if (id != null)
            {
                var r = await (from p in _appContext.Questions
                               where p.ID == id

                               select new QuestionOutputDTO
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Expire = p.EXPIRE,
                                   IsAddAnswer = p.IS_ADD_ANSWER,
                                   IsMultiple = p.IS_MULTIPLE,
                                   Counter = (from b in _appContext.Answers
                                             join c in _appContext.AnswerUsers on b.ID equals c.ANSWER_ID
                                             where b.QUESTION_ID == p.ID
                                             select c.EMP_ID
                                             ).Distinct().Count(),
                                   Answers = (
                                                from a in _appContext.Answers
                                                where a.QUESTION_ID == p.ID
                                                select new AnswerOutputDTO
                                                {
                                                    Id = a.ID,
                                                    Answer = a.ANSWER,
                                                    IsVote = _appContext.AnswerUsers.Where(c => c.EMP_ID == _appContext.EmpId && c.ANSWER_ID == a.ID).Any(),                                                    
                                                    Employees = (
                                                                     from b in _appContext.AnswerUsers
                                                                     join e in _appContext.Employees on b.EMP_ID equals e.ID
                                                                     where b.ANSWER_ID == a.ID
                                                                     select new EmployeeOutputDTO
                                                                     {
                                                                         Id = b.EMP_ID,
                                                                         Avatar = e.Profile.AVATAR,
                                                                         Name = e.Profile.FULL_NAME
                                                                     }
                                                                 ).ToList()
                                                }
                                            ).ToList()
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            else
            {
                var r = await (from p in _appContext.Questions
                               
                               orderby p.ID descending
                               select new QuestionOutputDTO
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Expire = p.EXPIRE,
                                   IsAddAnswer = p.IS_ADD_ANSWER,
                                   IsMultiple = p.IS_MULTIPLE,
                                   Counter = (from b in _appContext.Answers
                                              join c in _appContext.AnswerUsers on b.ID equals c.ANSWER_ID
                                              where b.QUESTION_ID == p.ID
                                              select c.EMP_ID
                                             ).Distinct().Count(),
                                   Answers = (
                                                from a in _appContext.Answers
                                                where a.QUESTION_ID == p.ID
                                                select new AnswerOutputDTO
                                                {
                                                    Id = a.ID,
                                                    Answer = a.ANSWER,
                                                    IsVote = _appContext.AnswerUsers.Where(c => c.EMP_ID == _appContext.EmpId && c.ANSWER_ID == a.ID).Any(),
                                                    Employees = (
                                                                     from b in _appContext.AnswerUsers
                                                                     join e in _appContext.Employees on b.EMP_ID equals e.ID
                                                                     where b.ANSWER_ID == a.ID
                                                                     select new EmployeeOutputDTO
                                                                     {
                                                                         Id = b.EMP_ID,
                                                                         Avatar = e.Profile.AVATAR,
                                                                         Name = e.Profile.FULL_NAME
                                                                     }
                                                                 ).ToList()
                                                }
                                            ).ToList()
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }

        }
        /// <summary>
        /// Admin thêm câu hỏi va cac phuong án
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResultWithError> CreateQuestion(QuestionDTO param)
        {

            try
            {
                var data = Map(param, new HU_QUESTION());
                data.IS_ACTIVE = true;
                await _appContext.Questions.AddAsync(data);
                if (param.Answers != null && param.Answers.Count > 0)
                {
                    List<HU_ANSWER> listA = new();
                    foreach (var item in param.Answers)
                    {
                        var answer = Map(item, new HU_ANSWER());
                        answer.QUESTION_ID = data.ID;
                        answer.IS_ACTIVE = true;
                        listA.Add(answer);
                    }
                    _appContext.Answers.AddRange(listA);
                }

                await _appContext.SaveChangesAsync();

                return new ResultWithError(param);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
        /// <summary>
        /// CMS Get All Data
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResult<QuestionPagingDTO>> GetAll(QuestionPagingDTO param)
        {
            var queryable = from p in _appContext.Questions
                            
                            orderby p.EXPIRE descending
                            select new QuestionPagingDTO
                            {
                                Id = p.ID,
                                Name = p.NAME,
                                Expire = p.EXPIRE,
                                IsAddAnswer = p.IS_ADD_ANSWER,
                                IsMultiple = p.IS_MULTIPLE,
                                IsActive = p.IS_ACTIVE,
                                Results = (from c in _appContext.Answers
                                           where c.QUESTION_ID == p.ID 
                                           select new Result
                                           {
                                               Answer = c.ANSWER,
                                               Vote = _appContext.AnswerUsers.Where(a => a.ANSWER_ID == c.ID ).Count()
                                           }).ToList()
                            };

            if (param.Name != null)
            {
                queryable = queryable.Where(p => p.Name.ToUpper().Contains(param.Name.ToUpper()));
            }

            //if (param.IsActive != null)
            //{
            //    queryable = queryable.Where(p => p.IsActive == param.IsActive);
            //}
            return await PagingList(queryable, param);
        }

        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> GetById(long id)
        {
            try
            {
                var r = await (from p in _appContext.Questions
                               where p.ID == id
                               select new
                               {
                                   Id = p.ID,
                                   Name = p.NAME,
                                   Expire = p.EXPIRE,
                                   IsAddAnswer = p.IS_ADD_ANSWER,
                                   IsMultiple = p.IS_MULTIPLE,
                                   Answers = (
                                                from a in _appContext.Answers
                                                where a.QUESTION_ID == p.ID
                                                select new
                                                {
                                                    Id = a.ID,
                                                    Answer = a.ANSWER,
                                                    IsVote = _appContext.AnswerUsers.Where(c => c.EMP_ID == _appContext.EmpId && c.ANSWER_ID == a.ID).Any(),
                                                    Vote = _appContext.AnswerUsers.Where(b => b.ANSWER_ID == a.ID ).Count(),
                                                    Employees = (
                                                                     from b in _appContext.AnswerUsers
                                                                     join e in _appContext.Employees on b.EMP_ID equals e.ID
                                                                     where b.ANSWER_ID == a.ID
                                                                     select new EmployeeOutputDTO
                                                                     {
                                                                         Id = b.EMP_ID,
                                                                         Avatar = e.Profile.AVATAR,
                                                                         Name = e.Profile.FULL_NAME
                                                                     }
                                                                 ).ToList()
                                                }
                                            ).ToList()
                               }).FirstOrDefaultAsync();
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {

                return new ResultWithError(ex.Message);
            }

        }
        /// <summary>
        /// CMS Change Status Data
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<ResultWithError> ChangeStatusAsync(List<long> ids)
        {
            foreach (var item in ids)
            {
                var r = _appContext.Questions.Where(x => x.ID == item).FirstOrDefault();
                if (r == null)
                {
                    return new ResultWithError(404);
                }
                r.IS_ACTIVE = !r.IS_ACTIVE;
                var result = _appContext.Questions.Update(r);
            }
            await _appContext.SaveChangesAsync();
            return new ResultWithError(200);
        }
        /// <summary>
        /// CMS Get Detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public async Task<ResultWithError> PortalList()
        {
            try
            {
                var r = await QueryData.ExecuteStore<PortalVoteParam>(Procedures.PKG_PORTAL_VOTE_LIST, new
                {
                    
                    P_CUR = QueryData.OUT_CURSOR
                });
                return new ResultWithError(r);
            }
            catch (Exception ex)
            {
                return new ResultWithError(ex.Message);
            }
        }
    }
}
