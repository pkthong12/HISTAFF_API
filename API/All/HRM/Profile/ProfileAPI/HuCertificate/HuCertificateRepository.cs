using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using System;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using CORE.Services.File;
using Microsoft.Extensions.Options;

namespace API.Controllers.HuCertificate
{
	public class HuCertificateRepository : IHuCertificateRepository
	{
		private readonly GenericUnitOfWork _uow;
		private readonly FullDbContext _dbContext;
		private IGenericRepository<HU_CERTIFICATE, HuCertificateDTO> _genericRepository;
		private readonly GenericReducer<HU_CERTIFICATE, HuCertificateDTO> _genericReducer;
		private readonly IWebHostEnvironment _env;
		private readonly AppSettings _appSettings;
		private readonly IFileService _fileService;
		public HuCertificateRepository(FullDbContext context, GenericUnitOfWork uow,
						IWebHostEnvironment env,
			IOptions<AppSettings> options,
			IFileService fileService)
		{
			_dbContext = context;
			_uow = uow;
			_genericRepository = _uow.GenericRepository<HU_CERTIFICATE, HuCertificateDTO>();
			_genericReducer = new();
			_env = env;
			_appSettings = options.Value;
			_fileService = fileService;
		}

		public async Task<GenericPhaseTwoListResponse<HuCertificateDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCertificateDTO> request)
		{
			var joined = from p in _dbContext.HuCertificates.AsNoTracking()
						 from e in _dbContext.HuEmployees.Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()
						 from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
						 from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
						 from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
						 from j in _dbContext.HuJobs.Where(x => x.ID == t.JOB_ID).DefaultIfEmpty()
						 from type in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_CERTIFICATE).DefaultIfEmpty()
						 from lvtrain in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_TRAIN).DefaultIfEmpty()
						 from typeTrain in _dbContext.SysOtherLists.Where(x => x.ID == p.TYPE_TRAIN).DefaultIfEmpty()
						 from school in _dbContext.SysOtherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
						 from lv in _dbContext.SysOtherLists.Where(x => x.ID == p.LEVEL_ID).DefaultIfEmpty()
						 orderby p.YEAR descending
						 select new HuCertificateDTO
						 {
							 Id = p.ID,
							 EmployeeCode = e.CODE,
                             WorkStatusId = e.WORK_STATUS_ID,
                             EmployeeFullName = cv.FULL_NAME,
							 OrgName = o.NAME,
							 TitleName = t.NAME,
							 TypeCertificateName = type.NAME,
							 Name = p.NAME,
							 TrainFromDate = p.TRAIN_FROM_DATE,
							 TrainToDate = p.TRAIN_TO_DATE,
							 EffectFrom = p.EFFECT_FROM,
							 EffectTo = p.EFFECT_TO,
							 LevelTrainName = lvtrain.NAME,
							 Major = p.MAJOR,
							 ContentTrain = p.CONTENT_TRAIN,
							 SchoolName = school.NAME,
							 Year = p.YEAR,
                             YearStr = p.YEAR.ToString(),
                             Mark = p.MARK,
                             MarkStr = p.MARK.ToString(),
                             IsPrime = p.IS_PRIME,
							 IsLicense = p.IS_LICENSE,
							 Level = p.LEVEL,
							 TypeTrainName = typeTrain.NAME,
							 Remark = p.REMARK,
							 LevelId= p.LEVEL_ID,
							 LevelName= lv.NAME,
							 OrgId= e.ORG_ID,
							 JobOderNum = Convert.ToInt32(j.ORDERNUM ?? 999)
						 };

			var singlePhaseResult = await _genericReducer.SinglePhaseReduce(joined, request);
			return singlePhaseResult;
		}

		public async Task<FormatedResponse> ReadAll()
		{
			var response = await _genericRepository.ReadAll();
			return new FormatedResponse() { InnerBody = response};
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
					var list = new List<HU_CERTIFICATE>
					{
						(HU_CERTIFICATE)response
					};
					var joined = (from p in list
								  select new HuCertificateDTO
								  {
									  Id = p.ID,
									  Name = p.NAME,
									  TrainFromDate = p.TRAIN_FROM_DATE,
									  TrainToDate = p.TRAIN_TO_DATE,
									  EffectFrom = p.EFFECT_FROM,
                                      EffectTo = p.EFFECT_TO,
                                      Major = p.MAJOR,
									  ContentTrain = p.CONTENT_TRAIN,
									  Year = p.YEAR,
									  Mark = p.MARK,
									  IsPrime = p.IS_PRIME,
									  IsLicense	= p.IS_LICENSE,
									  Level = p.LEVEL,
									  Remark = p.REMARK,
									  EmployeeId= p.EMPLOYEE_ID,
									  TypeCertificate = p.TYPE_CERTIFICATE,
									  LevelTrain = p.LEVEL_TRAIN,
									  SchoolId = p.SCHOOL_ID,
									  Classification= p.CLASSIFICATION,
									  TypeTrain= p.TYPE_TRAIN,
									  FileName= p.FILE_NAME,
									  LevelId=  p.LEVEL_ID
								  }).FirstOrDefault();

					if (joined != null)
					{
						if (res.StatusCode == EnumStatusCode.StatusCode200)
						{
							var child = (from e in _dbContext.HuEmployees.Where(x => x.ID == joined.EmployeeId)
										 from cv in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
										 from t in _dbContext.HuPositions.Where(x => x.ID == e.POSITION_ID).DefaultIfEmpty()
										 from o in _dbContext.HuOrganizations.Where(x => x.ID == e.ORG_ID).DefaultIfEmpty()
										 from type in _dbContext.SysOtherLists.Where(x => x.ID == joined.TypeCertificate).DefaultIfEmpty()
										 from lvtrain in _dbContext.SysOtherLists.Where(x => x.ID == joined.LevelTrain).DefaultIfEmpty()
										 from typeTrain in _dbContext.SysOtherLists.Where(x => x.ID == joined.TypeTrain).DefaultIfEmpty()
										 from school in _dbContext.SysOtherLists.Where(x => x.ID == joined.SchoolId).DefaultIfEmpty()
										 from lv in _dbContext.SysOtherLists.Where(x => x.ID == joined.LevelId).DefaultIfEmpty()
										 select new
										 {
											 EmployeeCode = e.CODE,
											 EmployeeFullName = cv.FULL_NAME,
											 OrgName = o.NAME,
											 TitleName = t.NAME,
											 TypeCertificateName = type.NAME,
											 LevelTrainName = lvtrain.NAME,
											 SchoolName = school.NAME,
											 TypeTrainName = typeTrain.NAME,
											 LevelName= lv.NAME,
											 OrgId = e.ORG_ID
										 }).FirstOrDefault();

							joined.EmployeeCode = child?.EmployeeCode;
							joined.EmployeeFullName = child?.EmployeeFullName;
							joined.OrgName = child?.OrgName;
							joined.TitleName = child?.TitleName;
							joined.TypeCertificateName = child?.TypeCertificateName;
							joined.LevelTrainName = child?.LevelTrainName;
							joined.SchoolName = child?.SchoolName;
							joined.TypeTrainName = child?.TypeTrainName;
							joined.LevelName = child?.LevelName;
							joined.OrgId = child?.OrgId;
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

		public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuCertificateDTO dto, string sid)
		{

			_uow.CreateTransaction();
			List<UploadFileResponse> uploadFiles = new();
			// First of all we need to upload all the attachments
			if (dto.FirstAttachmentBuffer != null)
			{
				string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
				var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
				{
					ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
					ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
					ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
				}, location, sid);

				// Assign saved paths
				var property = typeof(HuCertificateDTO).GetProperty("FileName");

				if (property != null)
				{
					property?.SetValue(dto, uploadFileResponse.SavedAs);
					uploadFiles.Add(uploadFileResponse);
				}
				else
				{
					return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" };
				}

			}
			var response = await _genericRepository.Create(_uow, dto, sid);
			_uow.Commit();
            return new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                
				InnerBody = response.InnerBody,
                
				StatusCode = response.StatusCode
            };
        }

		public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuCertificateDTO> dtos, string sid)
		{
			var add = new List<HuCertificateDTO>();
			add.AddRange(dtos);
			var response = await _genericRepository.CreateRange(_uow, add, sid);
			return response;
		}

		public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuCertificateDTO dto, string sid, bool patchMode = true)
		{
			_uow.CreateTransaction();
			List<UploadFileResponse> uploadFiles = new();
			// First of all we need to upload all the attachments
			if (dto.FirstAttachmentBuffer != null)
			{
				string location = Path.Combine(_env.ContentRootPath, _appSettings.StaticFolders.Root, _appSettings.StaticFolders.Attachments);
				var uploadFileResponse = await _fileService.UploadFile(new UploadRequest()
				{
					ClientFileName = dto.FirstAttachmentBuffer.ClientFileName,
					ClientFileType = dto.FirstAttachmentBuffer.ClientFileType,
					ClientFileData = dto.FirstAttachmentBuffer.ClientFileData
				}, location, sid);

				// Assign saved paths
				var property = typeof(HuCertificateDTO).GetProperty("FileName");

				if (property != null)
				{
					property?.SetValue(dto, uploadFileResponse.SavedAs);
					uploadFiles.Add(uploadFileResponse);
				}
				else
				{
					return new() { ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400, MessageCode = CommonMessageCode.ASSIGN_TO_VALUE_DOES_NOT_MATCH_ANY_COLUMN + ": FirstAttachment" };
				}

			}

			//if(dto.L)

			var response = await _genericRepository.Update(_uow, dto, sid, patchMode);
			_uow.Commit();
            return new FormatedResponse()
            {
                MessageCode = response.MessageCode,
                InnerBody = response.InnerBody,
                StatusCode = response.StatusCode
            };
        }

		public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuCertificateDTO> dtos, string sid, bool patchMode = true)
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

        public virtual async Task<FormatedResponse> ToggleActiveIds(GenericUnitOfWork _uow, List<long> ids, bool valueToBind, string sid)
        {
            await Task.Run(() => null);
            throw new NotImplementedException();
        }

    }
}

