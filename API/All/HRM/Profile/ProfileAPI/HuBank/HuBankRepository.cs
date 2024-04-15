using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using Common.Extensions;
using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace API.Controllers.HuBank
{
    public class HuBankRepository : IHuBankRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_BANK, HuBankDTO> _genericRepository;
        private readonly GenericReducer<HU_BANK, HuBankDTO> _genericReducer;

        public HuBankRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_BANK, HuBankDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuBankDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuBankDTO> request)
        {
            var joined = from p in _dbContext.HuBanks.AsNoTracking()
                         select new HuBankDTO
                         {
                             Id = p.ID,
                             Code = p.CODE,
                             Name = p.NAME,
                             ShortName = p.SHORT_NAME,
                             IsActive = p.IS_ACTIVE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             Note = p.NOTE,
                             Order = p.ORDER,
                             CreatedBy = p.CREATED_BY,
                             CreatedDate = p.CREATED_DATE,
                             UpdatedBy = p.UPDATED_BY,
                             UpdatedDate = p.UPDATED_DATE
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            // var resposne = await _genericRepository.ReadAll();

            var resposne = await _dbContext.HuBanks.Where(x => x.IS_ACTIVE == true).ToListAsync();

            return new FormatedResponse()
            {
                InnerBody = resposne
            };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, long value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> ReadAllByKey(string key, string value)
        {
            var resposne = await _genericRepository.ReadAllByKey(key, value);
            return new() { InnerBody = resposne };
        }

        public async Task<FormatedResponse> GetById(long id)
        {
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<HU_BANK>
                    {
                        (HU_BANK)response
                    };
                var joined = (from l in list
                              select new HuBankDTO
                              {
                                  Id = l.ID,
                                  Code = l.CODE,
                                  Name = l.NAME,
                                  ShortName = l.SHORT_NAME,
                                  Note = l.NOTE,
                                  Order = l.ORDER,
                                  IsActive = l.IS_ACTIVE
                              }).FirstOrDefault();

                return new FormatedResponse() { InnerBody = joined };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetById(string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuBankDTO dto, string sid)
        {
            // không cho tạo trùng tên
            int existName = _dbContext.HuBanks.Where(b => b.NAME == dto.Name).Count();
            if (existName > 0)
            {
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE + ": NAME" };
            }

            //int existShortName = _dbContext.HuBanks.Where(b => b.SHORT_NAME == dto.ShortName).Count();
            //if(existShortName > 0)
            //{
            //    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE + ": SHORT_NAME" };
            //}

            //if (string.IsNullOrWhiteSpace(dto.Name))
            //{
            //    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NAME_NOT_BLANK };
            //}

            //if (string.IsNullOrWhiteSpace(dto.ShortName))
            //{
            //    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.SHORT_NAME_NOT_BLANK };
            //}

            var response = await _genericRepository.Create(_uow, dto, sid);


            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuBankDTO> dtos, string sid)
        {
            var add = new List<HuBankDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuBankDTO dto, string sid, bool patchMode = true)
        {
            // code cũ kiểm tra trùng tên
            //int existName = _dbContext.HuBanks.Where(b => b.NAME == dto.Name).Count();
            //if (existName > 0)
            //{
            //    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE + ": NAME" };
            //}

            //int existShortName = _dbContext.HuBanks.Where(b => b.SHORT_NAME == dto.ShortName).Count();
            //if (existShortName > 0)
            //{
            //    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE + ": SHORT_NAME" };
            //}

            //if (string.IsNullOrWhiteSpace(dto.Name))
            //{
            //    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.NAME_NOT_BLANK };
            //}

            //if (string.IsNullOrWhiteSpace(dto.ShortName))
            //{
            //    return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.SHORT_NAME_NOT_BLANK };
            //}



            // code mới kiểm tra trùng tên
            int existName = _dbContext.HuBanks.Where(b => b.NAME == dto.Name && b.ID != dto.Id).Count();

            if (existName > 0)
            {
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE + ": NAME" };
            }

            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuBankDTO> dtos, string sid, bool patchMode = true)
        {
            var response = await _genericRepository.UpdateRange(_uow, dtos, sid, patchMode);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, long id)
        {
            var response = await _genericRepository.Delete(_uow, id);
            return response;
        }

        public async Task<FormatedResponse> Delete(GenericUnitOfWork _uow, string id)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<long> ids)
        {
            foreach (var id in ids)
            {
                var item = await _dbContext.HuBanks.Where(x => x.ID == id).AsNoTracking().SingleOrDefaultAsync();
                if (item != null && item.IS_ACTIVE == true)
                {
                    _uow.Rollback();
                    return new FormatedResponse() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE };
                }
            }
            var response = await _genericRepository.DeleteIds(_uow, ids);
            return response;
        }

        public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get List Bank is Activce
        /// </summary>
        /// <returns></returns>
        public async Task<FormatedResponse> GetList()
        {
            var queryable = await (from p in _dbContext.HuBanks
                                   where p.IS_ACTIVE == true
                                   orderby p.CODE
                                   select new
                                   {
                                       Id = p.ID,
                                       Code = p.CODE,
                                       Name = p.NAME,
                                   }).ToListAsync();
            return new FormatedResponse() { InnerBody = queryable };
        }

        public async Task<FormatedResponse> CreateNewCode()
        {
            string newCode = "";
            if (await _dbContext.HuBanks.CountAsync() == 0)
            {
                newCode = "NH001";
            }
            else
            {
                string lastestData = _dbContext.HuBanks.OrderByDescending(t => t.CODE).First().CODE!.ToString();

                string newNumber = (Int32.Parse(lastestData.Substring(3)) + 1).ToString();
                while (newNumber.Length < 2)
                {
                    newNumber = "0" + newNumber;
                }
                newCode = lastestData.Substring(0, 3) + newNumber;
            }
            return new FormatedResponse() { InnerBody = new { Code = newCode } };

        }
        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }
    }

}

