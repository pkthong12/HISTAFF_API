using API.DTO;
using CORE.DTO;
using CORE.GenericUOW;

namespace API.Controllers.HuCertificateEdit
{
    public interface IHuCertificateEditRepository: IGenericRepository<HU_CERTIFICATE_EDIT, HuCertificateEditDTO>
    {
        Task<GenericPhaseTwoListResponse<HuCertificateEditDTO>> SinglePhaseQueryList(GenericQueryListDTO<HuCertificateEditDTO> request);


        // phê duyệt bằng cấp/chứng chỉ
        Task<FormatedResponse> ApproveHuCertificateEdit(GenericUnapprovePortalDTO request , string sid);

        // từ chối phê duyệt bằng cấp/chứng chỉ
        //Task<FormatedResponse> UnapproveHuCertificateEdit(GenericUnapprovePortalDTO request, string sid);

        // gửi yêu cầu phê duyệt sửa bằng cấp/chứng chỉ
        // truyền dữ liệu vào bảng tạm
        Task<FormatedResponse> SendUpdateHuCertificateEdit(HuCertificateEditDTO dto);

        // lấy các bản ghi
        // là loại phê duyệt thêm mới
        // trong bảng tạm HU_CERTIFICATE_EDIT
        // có đặc điểm:
        // 1. IS_SEND_PORTAL = true
        // 2. IS_APPROVE_PORTAL = false
        // 3. ID_HU_CERTIFICATE = null
        Task<FormatedResponse> GetPortalByEmployeeId(long id);

        /*
            lấy bản ghi theo ID
            mặc định sẽ lấy bản ghi theo ID của bảng HU_CERTIFICATE

            nếu xuất hiện IS_APPROVE_PORTAL = false
            thì lấy bản ghi theo ID của bảng HU_CERTIFICATE_EDIT

            tham số của phương thức GetByIdHuCertificateEdit()
            sẽ phải là HuCertificateEditModel
            bên trong model thì có 2 thuộc tính
            1. long? ID
            2. bool? IS_APPROVE_PORTAL
        */
        Task<FormatedResponse> GetByIdHuCertificateEdit(long? Id, string? StatusRecord);

        // khi bấm nút lưu
        Task<FormatedResponse> ClickSave(HuCertificateEditDTO model);

        // khai báo phương thức
        // để lấy danh sách trạng thái phê duyệt
        // để phục vụ cho Drop Down List
        Task<FormatedResponse> GetListNameOfApprove();

        // khai báo phương thức
        // để lấy tên của cái trạng thái
        // để phục vụ cho 1 ô input
        Task<FormatedResponse> GetListNameOfApproveById(long id);

        Task<FormatedResponse> GetListCertificate(long employeeId);
        Task<FormatedResponse> GetCertificateById(long id);
        Task<FormatedResponse> GetByIdCertificate(long id);
        Task<FormatedResponse> GetCertificateApproving(long employeeId);
        Task<FormatedResponse> GetCertificateSave(long employeeId);
        Task<FormatedResponse>GetCertificateUnapprove(long employeeId);
        Task<FormatedResponse> GetCertificateEditSaveById(long id);
        //Task<FormatedResponse> GetCertificateSaveById(long id);
        Task<FormatedResponse> GetByIdWebApp(long id);
    }
}

