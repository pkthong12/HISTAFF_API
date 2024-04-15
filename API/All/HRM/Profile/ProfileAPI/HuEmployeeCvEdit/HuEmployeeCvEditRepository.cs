using CORE.GenericUOW;
using CORE.DTO;
using API.DTO;
using CORE.Enum;
using CORE.StaticConstant;
using API.All.DbContexts;
using CORE.AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Common.Extensions;

namespace API.Controllers.HuEmployeeCvEdit
{
    public class HuEmployeeCvEditRepository : IHuEmployeeCvEditRepository
    {
        private readonly GenericUnitOfWork _uow;
        private readonly FullDbContext _dbContext;
        private IGenericRepository<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO> _genericRepository;
        private readonly GenericReducer<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO> _genericReducer;

        public HuEmployeeCvEditRepository(FullDbContext context, GenericUnitOfWork uow)
        {
            _dbContext = context;
            _uow = uow;
            _genericRepository = _uow.GenericRepository<HU_EMPLOYEE_CV_EDIT, HuEmployeeCvEditDTO>();
            _genericReducer = new();
        }

        public async Task<GenericPhaseTwoListResponse<HuEmployeeCvEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuEmployeeCvEditDTO> request)
        {
            // cái where IS_APPROVED_PORTAL == false
            // để lấy tất cả bản ghi
            // chưa được phê duyệt
            var joined = from p in _dbContext.HuEmployeeCvEdits.Where(x => x.IS_APPROVED_EDUCATION == false && x.STATUS_APPROVED_EDUCATION_ID == OtherConfig.STATUS_WAITING).AsNoTracking()
                         from emp in _dbContext.HuEmployees.AsNoTracking().Where(x => x.ID == p.EMPLOYEE_ID).DefaultIfEmpty()

                         // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                         from e in _dbContext.SysOtherLists.Where(x => x.ID == p.EDUCATION_LEVEL_ID).DefaultIfEmpty()
                         from z in _dbContext.SysOtherLists.Where(x => x.ID == p.LEARNING_LEVEL_ID).DefaultIfEmpty()
                         from q1 in _dbContext.SysOtherLists.Where(x => x.ID == p.QUALIFICATIONID).DefaultIfEmpty()
                         from q2 in _dbContext.SysOtherLists.Where(x => x.ID == p.QUALIFICATIONID_2).DefaultIfEmpty()
                         from q3 in _dbContext.SysOtherLists.Where(x => x.ID == p.QUALIFICATIONID_3).DefaultIfEmpty()
                         from t in _dbContext.SysOtherLists.Where(x => x.ID == p.TRAINING_FORM_ID).DefaultIfEmpty()
                         from t2 in _dbContext.SysOtherLists.Where(x => x.ID == p.TRAINING_FORM_ID_2).DefaultIfEmpty()
                         from t3 in _dbContext.SysOtherLists.Where(x => x.ID == p.TRAINING_FORM_ID_3).DefaultIfEmpty()
                         from s in _dbContext.SysOtherLists.Where(x => x.ID == p.SCHOOL_ID).DefaultIfEmpty()
                         from s2 in _dbContext.SysOtherLists.Where(x => x.ID == p.SCHOOL_ID_2).DefaultIfEmpty()
                         from s3 in _dbContext.SysOtherLists.Where(x => x.ID == p.SCHOOL_ID_3).DefaultIfEmpty()
                         from l1 in _dbContext.SysOtherLists.Where(x => x.ID == p.LANGUAGE_ID).DefaultIfEmpty()
                         from l2 in _dbContext.SysOtherLists.Where(x => x.ID == p.LANGUAGE_ID_2).DefaultIfEmpty()
                         from l3 in _dbContext.SysOtherLists.Where(x => x.ID == p.LANGUAGE_ID_3).DefaultIfEmpty()
                         from lv1 in _dbContext.SysOtherLists.Where(x => x.ID == p.LANGUAGE_LEVEL_ID).DefaultIfEmpty()
                         from lv2 in _dbContext.SysOtherLists.Where(x => x.ID == p.LANGUAGE_LEVEL_ID_2).DefaultIfEmpty()
                         from lv3 in _dbContext.SysOtherLists.Where(x => x.ID == p.LANGUAGE_LEVEL_ID_3).DefaultIfEmpty()

                         select new HuEmployeeCvEditDTO
                         {
                             // id của bảng HU_EMPLOYEE_CV_EDIT
                             Id = p.ID,
                             OrgId = emp.ORG_ID,
                             // Trình độ văn hóa
                             EducationLevelId = p.EDUCATION_LEVEL_ID,
                             EducationLevel = e.NAME,

                             // Trình độ học vấn
                             LearningLevelId = p.LEARNING_LEVEL_ID,
                             LearningLevel = z.NAME,

                             // Trình độ tin học
                             ComputerSkill = p.COMPUTER_SKILL,

                             // Bằng lái xe
                             License = p.LICENSE,

                             // Trình độ chuyên môn 1
                             Qualificationid = p.QUALIFICATIONID,
                             Qualification1 = q1.NAME,

                             // Trình độ chuyên môn 2
                             Qualificationid2 = p.QUALIFICATIONID_2,
                             Qualification2 = q2.NAME,

                             // Trình độ chuyên môn 3
                             Qualificationid3 = p.QUALIFICATIONID_3,
                             Qualification3 = q3.NAME,

                             // Hình thức đào tạo 1
                             TrainingFormId = p.TRAINING_FORM_ID,
                             TrainingForm1 = t.NAME,

                             // Hình thức đào tạo 2
                             TrainingFormId2 = p.TRAINING_FORM_ID_2,
                             TrainingForm2 = t2.NAME,

                             // Hình thức đào tạo 3
                             TrainingFormId3 = p.TRAINING_FORM_ID_3,
                             TrainingForm3 = t3.NAME,

                             // Trường học 1
                             School1 = s.NAME,

                             // Trường học 2
                             School2 = s2.NAME,

                             // Trường học 3
                             School3 = s3.NAME,

                             // Ngoại ngữ 1
                             Language1 = l1.NAME,

                             // Ngoại ngữ 2
                             Language2 = l2.NAME,

                             // Ngoại ngữ 3
                             Language3 = l3.NAME,

                             // Trình độ ngoại ngữ 1
                             LanguageLevel1 = lv1.NAME,

                             // Trình độ ngoại ngữ 2
                             LanguageLevel2 = lv2.NAME,

                             // Trình độ ngoại ngữ 3
                             LanguageLevel3 = lv3.NAME,
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
            var res = await _genericRepository.GetById(id);
            if (res.InnerBody != null)
            {
                var response = res.InnerBody;
                var list = new List<HU_EMPLOYEE_CV_EDIT>
                    {
                        (HU_EMPLOYEE_CV_EDIT)response
                    };
                var joined = (from l in list
                                  // JOIN OTHER ENTITIES BASED ON THE BUSINESS
                              select new HuEmployeeCvEditDTO
                              {
                                  Id = l.ID
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

        public async Task<FormatedResponse> Create(GenericUnitOfWork _uow, HuEmployeeCvEditDTO dto, string sid)
        {
            var response = await _genericRepository.Create(_uow, dto, sid);
            return response;
        }

        public async Task<FormatedResponse> CreateRange(GenericUnitOfWork _uow, List<HuEmployeeCvEditDTO> dtos, string sid)
        {
            var add = new List<HuEmployeeCvEditDTO>();
            add.AddRange(dtos);
            var response = await _genericRepository.CreateRange(_uow, add, sid);
            return response;
        }

        public async Task<FormatedResponse> Update(GenericUnitOfWork _uow, HuEmployeeCvEditDTO dto, string sid, bool patchMode = true)
        {
            try
            {
                sid = "";
                bool pathMode = true;
                var response = await _genericRepository.Update(_uow, dto, sid, patchMode);  
                return response;
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
            //// trường hợp 1:
            //// HU_EMPLOYEE_CV_ID = null
            //// tức là từ bản ghi gốc
            //// người dùng bấm gửi duyệt luôn
            //if (dto.HuEmployeeCvId == null)
            //{
            //    // thế thì phải thêm mới 1 bản ghi
            //    // 1. thiết lập IS_SEND_PORTAL = true
            //    // 2. thiết lập IS_APPROVED_PORTAL = false
            //    // 3. thiết lập IS_SEND_PORTAL_EDUCATION = true
            //    // 4. thiết lập HU_EMPLOYEE_CV_ID = dto.Id

            //    HU_EMPLOYEE_CV_EDIT item = new HU_EMPLOYEE_CV_EDIT()
            //    {
            //        EDUCATION_LEVEL_ID = dto.EducationLevelId,
            //        EMPLOYEE_ID = dto.EmployeeId,
            //        LEARNING_LEVEL_ID = dto.LearningLevelId,
            //        QUALIFICATIONID = dto.Qualificationid,
            //        QUALIFICATIONID_2 = dto.Qualificationid2,
            //        QUALIFICATIONID_3 = dto.Qualificationid3,
            //        TRAINING_FORM_ID = dto.TrainingFormId,
            //        TRAINING_FORM_ID_2 = dto.TrainingFormId2,
            //        TRAINING_FORM_ID_3 = dto.TrainingFormId3,

            //        // thiết lập IS_SEND_PORTAL = true
            //        IS_SEND_PORTAL = true,

            //        // thiết lập IS_APPROVED_PORTAL = false
            //        IS_APPROVED_PORTAL = false,

            //        // thiết lập IS_SEND_PORTAL_EDUCATION = true
            //        IS_SEND_PORTAL_EDUCATION = true,

            //        // thiết lập HU_EMPLOYEE_CV_ID = dto.Id
            //        HU_EMPLOYEE_CV_ID = dto.Id
            //    };


            //    // thêm vào ngữ cảnh context
            //    _dbContext.HuEmployeeCvEdits.Add(item);


            //    // lưu db
            //    _dbContext.SaveChanges();
            //}
            //else if (dto.HuEmployeeCvId != null || dto.HuEmployeeCvId > 0)
            //{
            //    // trường hợp 2:
            //    // HU_EMPLOYEE_CV_ID khác null
            //    // tức là người dùng từ bản ghi đã lưu
            //    // bấm gửi


            //    // trường hợp này đã có HU_EMPLOYEE_CV_ID rồi
            //    // 1. truy vấn bản ghi trong bảng tạm theo dto.Id
            //    // 2. đổ dữ liệu từ dto vào cái bản ghi vửa truy vấn
            //    // 3. thiết lập IS_SEND_PORTAL = true
            //    // 4. thiết lập IS_APPROVED_PORTAL = false
            //    // 5. thiết lập IS_SEND_PORTAL_EDUCATION = true
            //    // 6. xóa cái IS_SAVE_PORTAL đi, bằng cách gán IS_SAVE_PORTAL = null


            //    var record = (from data in _dbContext.HuEmployeeCvEdits.Where(x => x.ID == dto.Id)
            //               select data).First();


            //    // đổ dữ liệu từ dto vào bản ghi trong bảng tạm
            //    record.EDUCATION_LEVEL_ID = dto.EducationLevelId;
            //    record.EMPLOYEE_ID = dto.EmployeeId;
            //    record.LEARNING_LEVEL_ID = dto.LearningLevelId;
            //    record.QUALIFICATIONID = dto.Qualificationid;
            //    record.QUALIFICATIONID_2 = dto.Qualificationid2;
            //    record.QUALIFICATIONID_3 = dto.Qualificationid3;
            //    record.TRAINING_FORM_ID = dto.TrainingFormId;
            //    record.TRAINING_FORM_ID_2 = dto.TrainingFormId2;
            //    record.TRAINING_FORM_ID_3 = dto.TrainingFormId3;

            //    // thiết lập IS_SEND_PORTAL = true
            //    record.IS_SEND_PORTAL = true;

            //    // thiết lập IS_APPROVED_PORTAL = false
            //    record.IS_APPROVED_PORTAL = false;

            //    // thiết lập IS_SEND_PORTAL_EDUCATION = true
            //    record.IS_SEND_PORTAL_EDUCATION = true;

            //    // xóa cái IS_SAVE_PORTAL đi
            //    record.IS_SAVE_PORTAL = null;


            //    // lưu dữ liệu vào db
            //    _dbContext.SaveChanges();
            //}


            //// truy vấn bản ghi mới nhất trong bảng tạm
            //var record_top1 = await _dbContext.HuEmployeeCvEdits.OrderByDescending(x => x.ID).Take(1).FirstAsync();

            
            //return new FormatedResponse()
            //{
            //    InnerBody = record_top1
            //};
        }

        public async Task<FormatedResponse> UpdateRange(GenericUnitOfWork _uow, List<HuEmployeeCvEditDTO> dtos, string sid, bool patchMode = true)
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


        // khai báo phương thức ApproveEducationEdit()
        // để thay đổi IS_APPROVED_PORTAL = false
        // sang IS_APPROVED_PORTAL = true
        public async Task<FormatedResponse> ApproveEducationEdit(List<long>? listId)
        {
            // đầu tiên
            // phải sử dụng try ... catch ...
            // để bắt ngoại lệ
            try
            {
                // kiểm tra listId phải khác null
                // và listId phải có bản ghi
                // thì mới thực hiện công việc
                // thiết lập IS_APPROVED_PORTAL = true
                if (listId != null && listId.Count > 0)
                {
                    // truy vấn để lấy các bản ghi
                    // có ID tương ứng với ID
                    // của listId
                    // cụ thể: sử dụng phương thức Contains()
                    var dto = await _dbContext.HuEmployeeCvEdits.Where(x => listId.Contains(x.ID)).ToListAsync();


                    // sử dụng vòng lặp foreach()
                    // để thiết lập IS_APPROVED_PORTAL = true
                    // cho từng bản ghi truy vấn được
                    foreach (var item in dto)
                    {
                        // thiết lập "IS_APPROVED_PORTAL = true"
                        item.IS_APPROVED_PORTAL = true;

                        // lưu sự thay đổi
                        // cụ thể: gọi phương thức SaveChanges()
                        _dbContext.SaveChanges();


                        // lấy ra employee_id
                        var employee_id = item.EMPLOYEE_ID;


                        /*
                            lấy ra bản ghi
                            có "employee_id" trùng với ID trong bảng HU_EMPLOYEE
                            sau đó dùng PROFILE_ID
                            để tham chiếu đến bảng HU_EMPLOYEE_CV
                        */


                        // việc 1:
                        // lấy ra bản ghi trong bảng HU_EMPLOYEE
                        // mục đích là lấy PROFILE_ID
                        var profile_id = _dbContext.HuEmployees.Where(x => x.ID == employee_id).Select(x => x.PROFILE_ID).FirstOrDefault();


                        // việc 2:
                        // lấy bản ghi trong bảng HU_EMPLOYEE_CV
                        // dựa vào profile_id vừa tìm được
                        // lấy 1 bản ghi thì dùng FirstOrDefault()
                        var record = _dbContext.HuEmployeeCvs.Where(x => x.ID == profile_id).FirstOrDefault();


                        /*
                            gọi thử dữ liệu trong HU_EMPLOYEE_CV_EDIT lên
                            nếu sau khi SaveChanges()
                            mà dữ liệu nó "IS_APPROVED_PORTAL = true"
                            luôn ở trong vòng lặp foreach()
                            thì tốt quá
                            tiện tay thiết lập dữ liệu
                            từ bảng HU_EMPLOYEE_CV_EDIT vào bảng HU_EMPLOYEE_CV luôn
                        */

                        // đây là 1 bản ghi sau khi SaveChange()
                        var record_after_SaveChange = _dbContext.HuEmployeeCvEdits.Where(x => x.ID == item.ID).FirstOrDefault();


                        // khai báo biến "status"
                        // để lưu trạng thái của IS_APPROVED_PORTAL
                        // sau khi SaveChange()
                        // theo như tôi debug thì "status = true"
                        bool? status = record_after_SaveChange.IS_APPROVED_PORTAL;


                        // nếu status == true
                        // thì bản ghi đã được phê duyệt

                        // nếu bản ghi đã được phê duyệt
                        // thì chúng ta bắt đầu đưa dữ liệu
                        // từ bảng HU_EMPLOYEE_CV_EDIT
                        // sang bảng HU_EMPLOYEE_CV
                        if (status == true && record_after_SaveChange != null && record != null)
                        {
                            // lấy danh sách "tên các trường bị sửa"
                            List<string> danh_sach = record_after_SaveChange.MODEL_CHANGE
                                .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList();


                            foreach (var phanTu in danh_sach)
                            {
                                switch (phanTu)
                                {
                                    case "EducationLevelId":
                                        // đây là trường hợp 1
                                        // người dùng có sửa "Trình độ văn hóa"
                                        record.EDUCATION_LEVEL_ID = record_after_SaveChange.EDUCATION_LEVEL_ID;
                                        _dbContext.SaveChanges();
                                        break;

                                    case "LearningLevelId":
                                        // đây là trường hợp 2
                                        // người dùng có sửa "Trình độ học vấn"
                                        record.LEARNING_LEVEL_ID = record_after_SaveChange.LEARNING_LEVEL_ID;
                                        _dbContext.SaveChanges();
                                        break;

                                    case "Qualificationid":
                                        // đây là trường hợp 3
                                        // người dùng có sửa "Trình độ chuyên môn 1"
                                        record.QUALIFICATIONID = record_after_SaveChange.QUALIFICATIONID;
                                        _dbContext.SaveChanges();
                                        break;

                                    case "Qualificationid2":
                                        // đây là trường hợp 4
                                        // người dùng có sửa "Trình độ chuyên môn 2"
                                        record.QUALIFICATIONID_2 = record_after_SaveChange.QUALIFICATIONID_2;
                                        _dbContext.SaveChanges();
                                        break;

                                    case "Qualificationid3":
                                        // đây là trường hợp 5
                                        // người dùng có sửa "Trình độ chuyên môn 3"
                                        record.QUALIFICATIONID_3 = record_after_SaveChange.QUALIFICATIONID_3;
                                        _dbContext.SaveChanges();
                                        break;

                                    case "TrainingFormId":
                                        // đây là trường hợp 6
                                        // người dùng có sửa "Hình thức đào tạo 1"
                                        record.TRAINING_FORM_ID = record_after_SaveChange.TRAINING_FORM_ID;
                                        _dbContext.SaveChanges();
                                        break;

                                    case "TrainingFormId2":
                                        // đây là trường hợp 7
                                        // người dùng có sửa "Hình thức đào tạo 2"
                                        record.TRAINING_FORM_ID_2 = record_after_SaveChange.TRAINING_FORM_ID_2;
                                        _dbContext.SaveChanges();
                                        break;

                                    case "TrainingFormId3":
                                        // đây là trường hợp 8
                                        // người dùng có sửa "Hình thức đào tạo 3"
                                        record.TRAINING_FORM_ID_3 = record_after_SaveChange.TRAINING_FORM_ID_3;
                                        _dbContext.SaveChanges();
                                        break;

                                    default:
                                        break;
                                }
                            }
                        }
                    }


                    // lấy ra tất cả bản ghi trạng thái mới nhất
                    // sau khi đã SaveChanges()
                    var all_record = await _dbContext.HuEmployeeCvEdits.Where(x => (bool)x.IS_APPROVED_PORTAL).ToListAsync();

                    // đoạn code trên bị comment lại
                    // vì a Thắng chỉ cho cách hay hơn

                    // tức là sử dụng reloadFlag$
                    // ở code front end
                    // để load lại lưới ở giao diện
                    // đỡ phải call lại dữ liệu mới lên rồi đổ vào lưới


                    return new FormatedResponse()
                    {
                        MessageCode = CommonMessageCode.APPROVE_SUCCESS,
                        InnerBody = all_record
                    };
                }
                else
                {
                    // viết code cho trường hợp
                    // người dùng chưa chọn bản ghi
                    // mà đã bấm phê duyệt
                    return new FormatedResponse()
                    {
                        StatusCode = EnumStatusCode.StatusCode400,
                        MessageCode = "Bạn chưa chọn bản ghi để phê duyệt"
                    };
                }
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { InnerBody = ex.Message };
            }

        }

        public  async Task<FormatedResponse> GetHuEmployeeCvEditCvApproving(long employeeId)
        {
            try
            {
                var query = await(from table in _dbContext.HuEmployeeCvEdits.Where(x => x.EMPLOYEE_ID == employeeId).DefaultIfEmpty()
                                  from e in _dbContext.HuEmployees.Where(x => x.ID == table.EMPLOYEE_ID).DefaultIfEmpty()
                                  from c in _dbContext.HuEmployeeCvs.Where(x => x.ID == e.PROFILE_ID).DefaultIfEmpty()
                                  from sys in _dbContext.SysOtherLists.Where(x => x.ID == c.NATIONALITY_ID).DefaultIfEmpty()
                                  from sys2 in _dbContext.SysOtherLists.Where(x => x.ID == c.NATIVE_ID).DefaultIfEmpty()
                                  from sys3 in _dbContext.SysOtherLists.Where(x => x.ID == c.RELIGION_ID).DefaultIfEmpty()
                                  from sys4 in _dbContext.SysOtherLists.Where(x => x.ID == c.MARITAL_STATUS_ID).DefaultIfEmpty()
                                  where table.IS_SEND_PORTAL_CV == true
                                  select new
                                  {
                                      Id = table.ID,
                                      BirthPlace = c.BIRTH_PLACE,
                                      IdNo = c.ID_NO,
                                      IdDate = c.ID_DATE,
                                      Domicile = c.DOMICILE,
                                      IdPlace = c.ID_PLACE,
                                      TaxCode = c.TAX_CODE,
                                      TaxCodeDate = c.TAX_CODE_DATE,
                                      TaxCodeAddress = c.TAX_CODE_ADDRESS,
                                      Nationality = sys.NAME,
                                      Native = sys2.NAME,
                                      Religion = sys3.NAME,
                                      MaritalStatus = sys4.NAME,
                                      PassNo = c.PASS_NO,
                                      PassDate = c.PASS_DATE,
                                      PassExpire = c.PASS_EXPIRE,
                                      PassPlace = c.PASS_PLACE,
                                  }).ToListAsync();
                return new() { InnerBody = query };
            }
            catch (Exception ex)
            {

                return new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode500, ErrorType = EnumErrorType.UNCATCHABLE, MessageCode = ex.Message };
            }
        }



        // chức năng lưu
        public async Task<FormatedResponse> SaveEducation(HuEmployeeCvEditDTO request)
        {
            try
            {
                // kiểm tra có ID không
                // nếu có ID (tức là sửa bản ghi)
                // nếu không có ID (tức là thêm mới bản ghi)
                if (request.HuEmployeeCvId != null)
                {
                    // lấy bản ghi trong bảng tạm lên để sửa
                    var record = await (from item in _dbContext.HuEmployeeCvEdits
                                        where item.ID == request.Id
                                        select item).FirstAsync();


                    // sửa bản ghi
                    record.EDUCATION_LEVEL_ID = request.EducationLevelId;
                    record.EMPLOYEE_ID = request.EmployeeId;
                    record.LEARNING_LEVEL_ID = request.LearningLevelId;
                    record.QUALIFICATIONID = request.Qualificationid;
                    record.QUALIFICATIONID_2 = request.Qualificationid2;
                    record.QUALIFICATIONID_3 = request.Qualificationid3;
                    record.TRAINING_FORM_ID = request.TrainingFormId;
                    record.TRAINING_FORM_ID_2 = request.TrainingFormId2;
                    record.TRAINING_FORM_ID_3 = request.TrainingFormId3;


                    // lưu thẳng vào db
                    _dbContext.SaveChanges();
                }
                else if (request.HuEmployeeCvId == null)
                {
                    // nếu không có "HuEmployeeCvId"
                    // thì thêm 1 bản ghi mới


                    // tạo đối tượng item
                    // có kiểu dữ liệu HU_EMPLOYEE_CV_EDIT
                    HU_EMPLOYEE_CV_EDIT item = new HU_EMPLOYEE_CV_EDIT()
                    {
                        EMPLOYEE_ID = request.EmployeeId,
                        EDUCATION_LEVEL_ID = request.EducationLevelId,
                        LEARNING_LEVEL_ID = request.LearningLevelId,
                        QUALIFICATIONID = request.Qualificationid,
                        QUALIFICATIONID_2 = request.Qualificationid2,
                        QUALIFICATIONID_3 = request.Qualificationid3,
                        TRAINING_FORM_ID = request.TrainingFormId,
                        TRAINING_FORM_ID_2 = request.TrainingFormId2,
                        TRAINING_FORM_ID_3 = request.TrainingFormId3,

                        // khi lưu thì thiết lập IS_SAVE_PORTAL = true
                        // và thiết lập IS_APPROVED_PORTAL = false
                        IS_SAVE_PORTAL = true,
                        IS_APPROVED_PORTAL = false,

                        // thiết lập 
                        HU_EMPLOYEE_CV_ID = request.Id,
                    };


                    // thêm item vào ngữ cảnh context
                    _dbContext.HuEmployeeCvEdits.Add(item);


                    // lưu dữ liệu xuống db
                    _dbContext.SaveChanges();
                }


                // truy vấn bản ghi mới nhất trong bảng tạm ra
                var top1_record = (from item in _dbContext.HuEmployeeCvEdits.OrderByDescending(x => x.ID).Take(1)
                                   select item);

                // lấy 1 bản ghi đầu tiên (giống select top 1)
                // thì dùng .Take(1)


                return new() { InnerBody = top1_record };

                // in dữ liệu ra màn hình
                // trình độ học vấn ở portal
                // /api/PortalStaffProfile/GetEducationByPortal
            }
            catch (Exception ex)
            {
                return new FormatedResponse() { MessageCode = ex.Message, ErrorType = EnumErrorType.CATCHABLE, StatusCode = EnumStatusCode.StatusCode400 };
            }
        }

    }
}

