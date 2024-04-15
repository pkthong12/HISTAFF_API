using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using System.Linq.Dynamic.Core;
using DocumentFormat.OpenXml.Drawing;

namespace API.Controllers.InsRegion
{
    public class InsRegionRepository : IInsRegionRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<INS_REGION, InsRegionDTO> _genericRepository;
        private readonly GenericReducer<INS_REGION, InsRegionDTO> _genericReducer;

        public InsRegionRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<INS_REGION, InsRegionDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<InsRegionDTO>> SinglePhaseQueryList(GenericQueryListDTO<InsRegionDTO> request)
        {
            var joined = from p in _dbContext.InsRegions.AsNoTracking()
                             // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from o in _dbContext.SysOtherLists.AsNoTracking().Where(x => x.ID == p.AREA_ID).DefaultIfEmpty()
                         select new InsRegionDTO
                         {
                             Id = p.ID,
                             RegionCode = p.REGION_CODE,
                             AreaId = p.AREA_ID,
                             AreaName = o.NAME,
                             EffectDate = p.EFFECT_DATE,
                             ExprivedDate = p.EXPRIVED_DATE,
                             Money = p.MONEY,
                             Note = p.NOTE,
                             Status = p.IS_ACTIVE == true ? "Áp dụng" : "Ngừng áp dụng",
                             CeilingUi = p.CEILING_UI,
                             IsActive = p.IS_ACTIVE
                         };

            var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
            //singlePhaseResult.List = singlePhaseResult.List!.OrderByDescending(x => x.EffectDate);
            return singlePhaseResult;
        }

        public async Task<FormatedResponse> ReadAll()
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
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
                var list = new List<INS_REGION>
                    {
                        (INS_REGION)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.ID == l.AREA_ID).DefaultIfEmpty()
                              select new InsRegionDTO
                              {
                                  Id = l.ID,
                                  RegionCode = l.REGION_CODE,
                                  AreaId = l.AREA_ID,
                                  AreaName = o.NAME,
                                  EffectDate = l.EFFECT_DATE,
                                  ExprivedDate = l.EXPRIVED_DATE,
                                  Money = l.MONEY,
                                  Note = l.NOTE,
                                  CeilingUi = l.CEILING_UI,
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, InsRegionDTO dto, string sid)
        {
            try
            {
                DateTime? dateEffNew = dto.EffectDate!.Value.Date;
                bool check = true;
                //check trung vung
                var x = await _dbContext.InsRegions.AsNoTracking()
                        .Where(x => x.AREA_ID == dto.AreaId).ToListAsync();

                foreach (var item in x)
                {
                    if (dateEffNew >= item.EFFECT_DATE && dateEffNew <= item.EXPRIVED_DATE)
                    {
                        return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "EXP_OLD_MUST_LESS_THAN_EFF_NEW", StatusCode = EnumStatusCode.StatusCode400 };
                    }
                }
                //check ngay hieu luc - ngay het hieu luc
                if(dto.EffectDate >= dto.ExprivedDate)
                {
                    return new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.START_MUST_LESS_THAN_END,
                        InnerBody = dto,
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                string newCode = "";
                if (await _dbContext.InsRegions.CountAsync() == 0)
                {
                    newCode = "001";
                }
                else
                {
                    string lastestData = _dbContext.InsRegions.OrderByDescending(t => t.REGION_CODE).First().REGION_CODE!.ToString();

                    newCode = (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
                }
                dto.RegionCode = newCode;
                dto.IsActive = true;
                var response = await _genericRepository.Create(_uow, dto, sid);
                return response;
            }
            catch (Exception ex)
            {
                return new FormatedResponse()
                {
                    MessageCode = CommonMessageCode.POST_UPDATE_FAILLED,
                    ErrorType = EnumErrorType.CATCHABLE,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<InsRegionDTO> dtos, string sid)
        {
            var add = new List<InsRegionDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, InsRegionDTO dto, string sid, bool patchMode = true)
        {
            try
            {
                // lấy bản ghi dưới db
                var record = await _dbContext.InsRegions.Where(x => x.ID == dto.Id).AsNoTracking().FirstAsync();

                // kiểm tra test case
                // trường hợp người dùng sửa
                // nhưng không thay đổi "Ngày hiệu lực"
                // và "Ngày hết hiệu lực"
                if (dto.EffectDate == record.EFFECT_DATE && dto.ExprivedDate == record.EXPRIVED_DATE)
                {
                    var result = await _genericRepository.Update(_uow, dto, sid, patchMode);
                    return result;
                }

                var dateEffNew = dto.EffectDate!.Value.Date;
                var id = dto.Id;
                //check eff_date new > exp old
                var x = await _dbContext.InsRegions.AsNoTracking()
                        .Where(x => x.AREA_ID == dto.AreaId).ToListAsync();

                foreach (var item in x)//trung ngay het hieu luc cu voi ngay hieu luc moi
                {
                    if( id != item.ID)
                    {
                        if (item.EXPRIVED_DATE >= dateEffNew)
                        {
                            return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = "EXP_OLD_MUST_LESS_THAN_EFF_NEW", StatusCode = EnumStatusCode.StatusCode400 };
                        }
                    }
                }

                //check ngay hieu luc - ngay het hieu luc
                if (dto.EffectDate >= dto.ExprivedDate)
                {
                    return new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.START_MUST_LESS_THAN_END,
                        InnerBody = dto,
                        ErrorType = EnumErrorType.CATCHABLE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
                }

                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
                return response;
            }
            catch
            {
                return new FormatedResponse()
                {
                    ErrorType = EnumErrorType.CATCHABLE,
                    MessageCode = CommonMessageCode.POST_UPDATE_FAILLED,
                    StatusCode = EnumStatusCode.StatusCode500
                };
            }
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<InsRegionDTO> dtos, string sid, bool patchMode = true)
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
            // lấy các bản ghi
            // theo id
            var list_record = _dbContext.InsRegions.Where(x => ids.Contains(x.ID)).AsNoTracking().DefaultIfEmpty();
            //foreach(var item in ids)
            //{
            //    var listSalLevel = _dbContext.HuSalaryLevels.Where(x => x.REGION_ID == item).AsNoTracking().Count();
            //    //var listSalLevel = _dbContext.HuSalaryLevels.Where(x => x.REGION_ID == item).AsNoTracking().Count();//ho so luong
            //    if(listSalLevel > 0)
            //    {
            //        return new FormatedResponse()
            //        {
            //            ErrorType = EnumErrorType.CATCHABLE,
            //            MessageCode = "CAN_NOT_DELETE_RECORD_IS_USING",
            //            StatusCode = EnumStatusCode.StatusCode400
            //        };
            //    }
            //}

            // kiểm tra trạng thái
            foreach (var item in list_record)
            {
                // nếu bản ghi có trạng thái "áp dụng"
                // thì dừng luôn
                // không cho xóa
                if (item?.IS_ACTIVE == true)
                {
                    return new FormatedResponse()
                    {
                        ErrorType = EnumErrorType.CATCHABLE,
                        MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                        StatusCode = EnumStatusCode.StatusCode400
                    };
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

        public async Task<FormatedResponse> GetSalaryBasicByRegion(string code)
        {
            var joined = await (from n in _dbContext.InsRegions
                                from sys in _dbContext.SysOtherLists.Where(x => x.ID == n.AREA_ID).DefaultIfEmpty()
                                where sys.CODE == code && ((n.EFFECT_DATE < DateTime.Now && n.EXPRIVED_DATE > DateTime.Now) || (n.EXPRIVED_DATE == null && n.EFFECT_DATE < DateTime.Now))
                                select new
                                {
                                    MONEY = n.MONEY
                                }).FirstOrDefaultAsync();
            
            if (joined != null && joined.MONEY != null)
            { 
                return new FormatedResponse() { InnerBody = joined.MONEY };
            }
            else
            {
                return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

        public async Task<FormatedResponse> GetSysOrtherList()
        {
            var query = await (from o in _dbContext.SysOtherLists.AsNoTracking().Where(o => o.TYPE_ID == 43)
                               orderby o.ID
                               select new
                               {
                                   AreaId = o.ID,
                                   Name = o.NAME,
                               }).ToListAsync();
            return new FormatedResponse() { InnerBody = query };
        }


        // thêm code
        // cho chức năng thay đổi trạng thái
        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            
            return response;
        }

        public async Task<FormatedResponse> GetRegionByDateNow()
        {
            try
            {
                List<InsRegionDTO> list = new();
                var getRegion1 = await (from r in _dbContext.InsRegions.Where(x => x.IS_ACTIVE == true)
                                        from o in _dbContext.SysOtherLists.Where(x => x.ID == r.AREA_ID).DefaultIfEmpty()
                                        orderby o.NAME
                                        select new InsRegionDTO() {
                                            Id = r.ID,
                                            AreaName = o.NAME,
                                            OtherListCode = o.CODE,
                                            EffectDate = r.EFFECT_DATE,
                                            ExprivedDate = r.EXPRIVED_DATE,
                                            Money = r.MONEY
                                        }).ToListAsync();
                getRegion1.ForEach(item =>
                {
                    if(item.ExprivedDate != null && item.ExprivedDate > DateTime.Now && item.EffectDate <= DateTime.Now)
                    {
                        list.Add(item);
                    }
                    else if(item.EffectDate <= DateTime.Now && item.ExprivedDate == null)
                    {
                        list.Add(item);
                    }
                });

                Dictionary<string, InsRegionDTO> uniqueRegions = new Dictionary<string, InsRegionDTO>();

                foreach (var item in list)
                {
                    if (!uniqueRegions.ContainsKey(item.AreaName))
                    {
                        uniqueRegions.Add(item.AreaName, item);
                    }
                    else
                    {
                        var existingItem = uniqueRegions[item.AreaName];
                        if (item.EffectDate > existingItem.EffectDate)
                        {
                            uniqueRegions[item.AreaName] = item;
                        }
                    }
                }

                var result = uniqueRegions.Values.ToList();

                return new FormatedResponse() { InnerBody = result };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }
    }
}

