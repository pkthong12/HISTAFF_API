using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;

namespace API.Controllers.HuDistrict
{
	public class HuDistrictRepository : IHuDistrictRepository
	{
		private readonly GenericUnitOfWork _uow;
		private readonly FullDbContext _dbContext;
		private IGenericRepository<HU_DISTRICT, HuDistrictDTO> _genericRepository;
		private readonly GenericReducer<HU_DISTRICT, HuDistrictDTO> _genericReducer;

		public HuDistrictRepository(FullDbContext context, GenericUnitOfWork uow)
		{
			_dbContext = context;
			_uow = uow;
			_genericRepository = _uow.GenericRepository<HU_DISTRICT, HuDistrictDTO>();
			_genericReducer = new();
		}

		public async Task<GenericPhaseTwoListResponse<HuDistrictDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuDistrictDTO> request)
		{
			var joined = from p in _dbContext.HuDistricts.AsNoTracking()
						 from pro in _dbContext.HuProvinces.Where(x => x.ID == p.PROVINCE_ID).DefaultIfEmpty()
						 from na in _dbContext.HuNations.Where(x => x.ID == pro.NATION_ID).DefaultIfEmpty()
						 select new HuDistrictDTO
						 {
							 Id = p.ID,
							 Name = p.NAME,
							 Code = p.CODE,
							 ProvinceId = p.PROVINCE_ID,
							 ProvinceName = pro.NAME,
							 CreatedBy = p.CREATED_BY,
							 CreatedDate = p.CREATED_DATE,
							 UpdatedBy = p.UPDATED_BY,
							 UpdatedDate = p.UPDATED_DATE,
							 Note = p.NOTE,
							 IsActive = p.IS_ACTIVE,
							 IsActiveStr = p.IS_ACTIVE ? "Áp dụng" : "Ngừng áp dụng",
							 NationId = pro.NATION_ID,
							 NationName = na.NAME,
						 };

			var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
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
			try
			{
				var res = await _genericRepository.GetById(id);
				if (res.InnerBody != null)
				{
					var response = res.InnerBody;
					var list = new List<HU_DISTRICT>
					{
						(HU_DISTRICT)response
					};
					var joined = (from p in list
								  select new HuDistrictDTO
								  {
									  Id = p.ID,
									  Name = p.NAME,
									  Code = p.CODE,
									  ProvinceId = p.PROVINCE_ID,
									  IsActive = p.IS_ACTIVE,
									  CreatedBy = p.CREATED_BY,
									  CreatedDate = p.CREATED_DATE,
									  UpdatedBy = p.UPDATED_BY,
									  UpdatedDate = p.UPDATED_DATE,
									  Note = p.NOTE,
								  }).FirstOrDefault();

					if (joined != null)
					{
						if (res.StatusCode == EnumStatusCode.StatusCode200)
						{
							var child = (from e in _dbContext.HuProvinces.Where(x => x.ID == joined.ProvinceId)
										 from na in _dbContext.HuNations.Where(x => x.ID == joined.NationId).DefaultIfEmpty()
										 select new
										 {
											 ProvinceName = e.NAME,
											 NationName = na.NAME,
											 NationId = e.NATION_ID
										 }).FirstOrDefault();
							joined.ProvinceName = child?.ProvinceName;
							joined.NationName = child?.NationName;
							joined.NationId = child?.NationId;
							return new FormatedResponse() { InnerBody = joined };
						}
						else
						{
							return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.GET_LIST_BY_KEY_RESPOSNE_IS_NOT_OF_STATUS_CODE_200, StatusCode = EnumStatusCode.StatusCode400 };
						}
					}
					else
					{
						return new() { ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.JOINED_QUERY_AFTER_GET_BY_ID_WAS_NULL, StatusCode = EnumStatusCode.StatusCode400 };
					}
				}
				else
				{
					return new FormatedResponse() { MessageCode = CommonMessageCode.ENTITY_NOT_FOUND, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
				}

			}
			catch (Exception ex)
			{
				return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.UNCATCHABLE, StatusCode = EnumStatusCode.StatusCode500 };

			}
		}

		public async Task<FormatedResponse> GetById(string id)
		{
			await Task.Run(() => null);
			throw new NotImplementedException();
		}

		public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuDistrictDTO dto, string sid)
		{

			var dis = _dbContext.HuDistricts.Where(x => x.PROVINCE_ID == dto.ProvinceId && x.NAME.Trim().ToLower() == dto.Name.Trim().ToLower()).FirstOrDefault();
			if (dis != null)
			{
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE };
            }

            string newCode = "";
			if (await _dbContext.HuDistricts.CountAsync() == 0)
			{
				newCode = "QH001";
			}
			else
			{
				string lastestData = _dbContext.HuDistricts.OrderByDescending(t => t.CODE).First().CODE!.ToString();

				newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
			}
			dto.Code = newCode;
			dto.IsActive = true;

			var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

		public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuDistrictDTO> dtos, string sid)
		{
			var add = new List<HuDistrictDTO>();
			add.AddRange(dtos);
			var response = await _genericRepository.CreateRange(_uow, add, sid);
			return response;
		}

		public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuDistrictDTO dto, string sid, bool patchMode = true)
		{

			var dis = _dbContext.HuDistricts.Where(x => x.PROVINCE_ID == dto.ProvinceId && x.NAME.Trim().ToLower() == dto.Name.Trim().ToLower()&& x.ID!= dto.Id).FirstOrDefault();
			if (dis != null)
			{
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE };
            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

		public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuDistrictDTO> dtos, string sid, bool patchMode = true)
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
                var response = await _genericRepository.DeleteIds(_uow, ids);
                return response;
        }

		public async Task<FormatedResponse> DeleteIds(GenericUnitOfWork _uow, List<string> ids)
		{
			await Task.Run(() => null);
			throw new NotImplementedException();
		}
		public async Task<FormatedResponse> CreateNewCode()
		{
			string newCode = "";
			if (await _dbContext.HuDistricts.CountAsync() == 0)
			{
				newCode = "QH001";
			}
			else
			{
				string lastestData = _dbContext.HuDistricts.OrderByDescending(t => t.CODE).First().CODE!.ToString();

				newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 3)) + 1).ToString("D3");
			}

			return new FormatedResponse() { InnerBody = new { Code = newCode } };

		}

        public async Task<FormatedResponse> CheckActive(List<long> ids)
        {
            var checkActive = (await _dbContext.HuDistricts.Where(x => ids.Contains(x.ID) && x.IS_ACTIVE == true).CountAsync() > 0) ? true : false;
            var checkDistint = (await _dbContext.HuWards.Where(x => ids.Contains((long)x.DISTRICT_ID)).CountAsync() > 0) ? true : false;
            var checkHuEmpCv = (await _dbContext.HuEmployeeCvs.Where(x => ids.Contains(x.DISTRICT_ID != null ? (long)x.DISTRICT_ID : 0)).CountAsync() > 0) ? true : false;
            var checkFamily = (await _dbContext.HuFamilys.Where(x => ids.Contains(x.BIRTH_CER_DISTRICT != null ? (long)x.BIRTH_CER_DISTRICT : 0)).CountAsync() > 0) ? true : false;
            var checkWhereHealth = (await _dbContext.InsWhereHealThs.Where(x => ids.Contains(x.DISTRICT_ID)).CountAsync() > 0) ? true : false;

            if (checkActive)
            {
                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode400,
                    MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                    ErrorType = EnumErrorType.CATCHABLE,
                };
            }
            if (checkDistint || checkHuEmpCv || checkFamily|| checkWhereHealth)
            {
                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode400,
                    MessageCode = CommonMessageCode.DATA_HAS_USED,
                    ErrorType = EnumErrorType.CATCHABLE,
                };
            }
            else
            {
                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode200,
                };
            }
        }

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            var response = await _genericRepository.ToggleActiveIds(_uow, ids, valueToBind, sid);
            return response;
        }


    }
}

