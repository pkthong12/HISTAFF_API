using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using ProfileDAL.ViewModels;

namespace API.Controllers.HuWard
{
	public class HuWardRepository : IHuWardRepository
	{
		private readonly GenericUnitOfWork _uow;
		private readonly FullDbContext _dbContext;
		private IGenericRepository<HU_WARD, HuWardDTO> _genericRepository;
		private readonly GenericReducer<HU_WARD, HuWardDTO> _genericReducer;

		public HuWardRepository(FullDbContext context, GenericUnitOfWork uow)
		{
			_dbContext = context;
			_uow = uow;
			_genericRepository = _uow.GenericRepository<HU_WARD, HuWardDTO>();
			_genericReducer = new();
		}

		public async Task<GenericPhaseTwoListResponse<HuWardDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuWardDTO> request)
		{
			var joined = from p in _dbContext.HuWards.AsNoTracking()
						 from dis in _dbContext.HuDistricts.Where(x => x.ID == p.DISTRICT_ID).DefaultIfEmpty()
						 from pro in _dbContext.HuProvinces.Where(x => x.ID == dis.PROVINCE_ID).DefaultIfEmpty()
						 from na in _dbContext.HuNations.Where(x => x.ID == pro.NATION_ID).DefaultIfEmpty()
						 select new HuWardDTO
						 {
							 Id = p.ID,
							 Name = p.NAME,
							 Code = p.CODE,
							 IsActive = p.IS_ACTIVE,
							 CreatedBy = p.CREATED_BY,
							 CreatedDate = p.CREATED_DATE,
							 UpdatedBy = p.UPDATED_BY,
							 UpdatedDate = p.UPDATED_DATE,
							 DistrictId = p.DISTRICT_ID,
							 DistrictName = dis.NAME,
							 ProvinceId = dis.PROVINCE_ID,
							 ProvinceName = pro.NAME,
							 Note = p.NOTE,
							 Status = p.IS_ACTIVE ? "Áp dụng" : "Ngừng áp dụng",
							 NationId = pro.NATION_ID,
							 NationName = na.NAME
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
					var list = new List<HU_WARD>
					{
						(HU_WARD)response
					};
					var joined = (from p in list
                                  select new HuWardDTO
								  {
									  Id = p.ID,
									  Name = p.NAME,
									  Code = p.CODE,
									  IsActive = p.IS_ACTIVE,
									  CreatedBy = p.CREATED_BY,
									  CreatedDate = p.CREATED_DATE,
									  UpdatedBy = p.UPDATED_BY,
									  UpdatedDate = p.UPDATED_DATE,
									  Note = p.NOTE,
									  DistrictId = p.DISTRICT_ID,
								  }).FirstOrDefault();

					if (joined != null)
					{
						if (res.StatusCode == EnumStatusCode.StatusCode200)
						{
							var child = (from e in _dbContext.HuDistricts.Where(x => x.ID == joined.DistrictId)
										 from pro in _dbContext.HuProvinces.Where(x => x.ID == e.PROVINCE_ID).DefaultIfEmpty()
										 from na in _dbContext.HuNations.Where(x => x.ID == pro.NATION_ID).DefaultIfEmpty()
										 select new
										 {
											 DistrictId = e.ID,
											 DistrictName = e.NAME,
											 ProvinceId = e.PROVINCE_ID,
											 ProvinceName = pro.NAME,
											 NationId = na.ID,
											 NationName = na.NAME
										 }).FirstOrDefault();

							joined.ProvinceName = child?.ProvinceName;
							joined.DistrictName = child?.DistrictName;
							joined.ProvinceId = child?.ProvinceId;
							joined.DistrictId = child?.DistrictId;
							joined.NationId = child?.NationId;
							joined.NationName = child?.NationName;
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

		public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuWardDTO dto, string sid)
		{

			var wa = _dbContext.HuWards.Where(x => x.DISTRICT_ID == dto.DistrictId && x.NAME.Trim().ToLower() == dto.Name.Trim().ToLower()).FirstOrDefault();
			if (wa != null)
			{
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE };
            }

            string newCode = "";
			if (await _dbContext.HuWards.CountAsync() == 0)
			{
				newCode = "XP00001";
			}
			else
			{
				string lastestData = _dbContext.HuWards.OrderByDescending(t => t.CODE).First().CODE!.ToString();

				newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 5)) + 1).ToString("D5");
			}
			dto.Code = newCode;
			dto.IsActive = true;
			var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

		public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuWardDTO> dtos, string sid)
		{
			var add = new List<HuWardDTO>();
			add.AddRange(dtos);
			var response = await _genericRepository.CreateRange(_uow, add, sid);
			return response;
		}

		public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuWardDTO dto, string sid, bool patchMode = true)
		{

			var wa = _dbContext.HuWards.Where(x => x.DISTRICT_ID == dto.DistrictId && x.NAME.Trim().ToLower() == dto.Name.Trim().ToLower()&&x.ID!= dto.Id).FirstOrDefault();
			if (wa != null)
			{
                return new() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = CommonMessageCode.DUBLICATE_VALUE };
            }
            var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
            return response;
        }

		public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuWardDTO> dtos, string sid, bool patchMode = true)
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
			if (await _dbContext.HuWards.CountAsync() == 0)
			{
				newCode = "XP00001";
			}
			else
			{
				string lastestData = _dbContext.HuWards.OrderByDescending(t => t.CODE).First().CODE!.ToString();

				newCode = lastestData.Substring(0, 2) + (int.Parse(lastestData.Substring(lastestData.Length - 5)) + 1).ToString("D5");
			}

			return new FormatedResponse() { InnerBody = new { Code = newCode } };

		}

        public async Task<FormatedResponse> CheckActive(List<long> ids)
        {
            var checkActive = (await _dbContext.HuWards.Where(x => ids.Contains(x.ID) && x.IS_ACTIVE == true).CountAsync() > 0) ? true : false;
			var checkHuEmpCv = (await _dbContext.HuEmployeeCvs.Where(x => ids.Contains(x.WARD_ID != null || x.CUR_WARD_ID != null ? (long)x.WARD_ID! : 0)).CountAsync() > 0) ? true : false;
            var checkFamily = (await _dbContext.HuFamilys.Where(x => ids.Contains(x.BIRTH_CER_WARD != null ? (long)x.BIRTH_CER_WARD : 0)).CountAsync() > 0) ? true : false;

            if (checkActive)
            {
                return new FormatedResponse()
                {
                    StatusCode = EnumStatusCode.StatusCode400,
                    MessageCode = CommonMessageCode.DO_NOT_DELETE_DATA_IS_ACTIVE,
                    ErrorType = EnumErrorType.CATCHABLE,
                };
            }
            if (  checkHuEmpCv || checkFamily )
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

        //// triển khai
        //// phương thức submit áp dụng
        //public async Task<FormatedResponse> SubmitActivate(List<long>? Ids)
        //{
        //    // đầu tiên
        //    // phải sử dụng try ... catch ...
        //    // để bắt ngoại lệ
        //    try
        //    {
        //        // kiểm tra Ids phải khác null
        //        // và Ids phải có bản ghi
        //        // thì mới thực hiện công việc
        //        // thiết lập IS_ACTIVE = true
        //        if (Ids != null && Ids.Count() > 0)
        //        {
        //            // truy vấn để lấy các bản ghi
        //            // có ID tương ứng với ID
        //            // của Ids
        //            // cụ thể: sử dụng phương thức Contains()
        //            var list_record = await _dbContext.HuWards.Where(x => Ids.Contains(x.ID) && x.IS_ACTIVE == false).ToListAsync();


        //            // sử dụng vòng lặp foreach()
        //            // để thiết lập IS_ACTIVE = true
        //            // cho từng bản ghi truy vấn được
        //            foreach (var item in list_record)
        //            {
        //                // thiết lập "IS_ACTIVE = true"
        //                item.IS_ACTIVE = true;

        //                // lưu sự thay đổi
        //                // cụ thể: gọi phương thức SaveChanges()
        //                _dbContext.SaveChanges();
        //            }

        //            return new FormatedResponse()
        //            {
        //                StatusCode = EnumStatusCode.StatusCode200,
        //                MessageCode = "Thiết lập trạng thái áp dụng thành công"
        //            };
        //        }
        //        else
        //        {
        //            // viết code cho trường hợp
        //            // người dùng chưa chọn bản ghi
        //            // mà đã bấm phê duyệt
        //            return new FormatedResponse()
        //            {
        //                StatusCode = EnumStatusCode.StatusCode400,
        //                MessageCode = "Bạn chưa chọn bản ghi để thiết lập trạng thái áp dụng"
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new FormatedResponse() { InnerBody = ex.Message };
        //    }
        //}


        //// triển khai
        //// phương thức submit ngừng áp dụng
        //public async Task<FormatedResponse> SubmitStopActivate(List<long>? Ids)
        //{
        //    // đầu tiên
        //    // phải sử dụng try ... catch ...
        //    // để bắt ngoại lệ
        //    try
        //    {
        //        // kiểm tra Ids phải khác null
        //        // và Ids phải có bản ghi
        //        // thì mới thực hiện công việc
        //        // thiết lập IS_ACTIVE = false
        //        if (Ids != null && Ids.Count() > 0)
        //        {
        //            // truy vấn để lấy các bản ghi
        //            // có ID tương ứng với ID
        //            // của Ids
        //            // cụ thể: sử dụng phương thức Contains()
        //            var list_record = await _dbContext.HuWards.Where(x => Ids.Contains(x.ID) && x.IS_ACTIVE == true).ToListAsync();


        //            // sử dụng vòng lặp foreach()
        //            // để thiết lập IS_ACTIVE = false
        //            // cho từng bản ghi truy vấn được
        //            foreach (var item in list_record)
        //            {
        //                // thiết lập "IS_ACTIVE = false"
        //                item.IS_ACTIVE = false;

        //                // lưu sự thay đổi
        //                // cụ thể: gọi phương thức SaveChanges()
        //                _dbContext.SaveChanges();
        //            }

        //            return new FormatedResponse()
        //            {
        //                StatusCode = EnumStatusCode.StatusCode200,
        //                MessageCode = "Thiết lập trạng thái ngừng áp dụng thành công"
        //            };
        //        }
        //        else
        //        {
        //            // viết code cho trường hợp
        //            // người dùng chưa chọn bản ghi
        //            // mà đã bấm phê duyệt
        //            return new FormatedResponse()
        //            {
        //                StatusCode = EnumStatusCode.StatusCode400,
        //                MessageCode = "Bạn chưa chọn bản ghi để thiết lập trạng ngừng thái áp dụng"
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new FormatedResponse() { InnerBody = ex.Message };
        //    }
        //}

    }
}

