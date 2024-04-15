namespace API.All.SYSTEM.CoreAPI.Xlsx
{

    /*************************************************************************
    * xlsx.json phải tuân thủ theo các classes được định nghĩa trong tệp này
    **************************************************************************/

    public enum EnumExType
    {
        EX_STRING = 1, EX_NUMBER = 2, EX_BOOLEAN = 3, EX_DATE = 4
    }

    public class InfoCell
    {
        public string Text { get; set; } = null!;
        public string? Comment { get; set; }
    }

    public class ExColumnRule
    {
        public required string Column { get; set; }
        public string? SysOtherListTypeCode { get; set; }
        public string? DirectReference { get; set; } // Dropdown của cột hiện tại lấy trực tiếp
        public string? ConsequentReference { get; set; } // Reference để truy vấn VLOOKUP
        public int? ConsequentBaseColumnR1C1Offset { get; set; } // Độ lệch cột tương đối tới cột tham số trong FormulaR1C1 style, yêu cầu là số nguyên âm
        public long? IndirectValue { get; set; } // Dropdown của cột hiện tại phụ thuộc vào giá trị gán cứng này. Ví dụ mặc định lấy tất cả các tỉnh có NATION_ID=1 (Vietnam)
        public string? IndirectColumn { get; set; } // Dropdown của cột hiện tại phụ thuộc vào giá trị (number) của cột này
        public List<string>? IndirectReference { get; set; } // The list contains 2 strings: ParentTable & ChildTable
        public bool? OrgTreeReference { get; set; } // Dropdown của cột hiện tại lấy từ OrgTree
        public bool? TimeRequired { get; set; } // Kiểu ngày tháng có thời gian 'dd/MM/yy hh:mm hoặc hh:mm:ss
        public bool? TimeOnly { get; set; } // Kiểu chỉ có thời gian hh:mm hoặc hh:mm:ss
    }

    // Tạo range tham chiếu trực tiếp (DIRECT) từ một bảng không cần lọc
    public class ExDirectReference
    {
        /* 
        *  Tên của vùng tham chiếu được tạo động = <TableName>
        *  Ví dụ HU_BANK
        *  Mỗi vùng tham chiếu cần có:
        *       1)  Cột thứ nhất là hợp nhất các giá trị của UniqueIndexColumns nối với nhau sử dụng ký tự, ví dụ "[Việt Nam] [Vĩnh Phúc]"
        *           Nếu UniqueIndexColumns chỉ có một column thì ký tự [] bị bỏ qua, ví dụ: "Ngân hàng Á Châu"
        *       2)  Cột thứ hai là ID (kiểu long) là giá trị khóa chính của bảng
        *  Tên của vùng tìm kiếm được tạo động = <TableName> + "_LOOKUP"
        *  Ví dụ HU_BANK_LOOKUP
        *************************************************************/

        public required string Table { get; set; }

        // Số lượng càng ít càng dễ nhìn
        // Nên tìm cách đặt Unique Index ngay trong DB
        public required List<string> UniqueIndexColumns { get; set; }
        public string? Condition { get; set; } = "1=1";

    }

    // Tạo range VLOOKUP hiển thị thêm thông tin dựa vào cột khác
    public class ExConsequentReference
    {
        /* 
        *  Tên của vùng tham chiếu được tạo động = <TableName>
        *  Ví dụ HU_BANK
        *  Mỗi vùng tham chiếu cần có:
        *       1)  Cột thứ nhất là ID (kiểu long) là giá trị khóa chính của bảng
        *       2)  Cột thứ hai là giá trị cần tìm
        *
        *  Tên của vùng tìm kiếm được tạo động = CustomRefrenceName + "__LOOKUP"
        *  Ví dụ HU_SALARY_SCALE__NAME__LOOKUP
        *************************************************************/

        public required string Table { get; set; }
        public string? Condition { get; set; } = "1=1";
        public required string ColumnToLookup { get; set; }
        public required string ConsequentReferenceName { get; set; }

    }

    // Tạo range tham chiếu gián tiếp (INDIRECT) từ bảng con dựa vào ID của bảng cha
    public class ExIndirectReference
    {
        /* Tên của vùng tham chiếu được tạo động = <ParentTable> + "_ID_" + <ChildTable>
        *  Ví dụ HU_PROVINCE_56_HU_DISTRIC sẽ là các huyện có ID tỉnh là 56
        *  Bảng cha cần có khóa chính là ID
        *  Mỗi vùng tham chiếu cần có:
        *       1)  Cột thứ nhất là hợp nhất các giá trị của UniqueIndexColumns nối với nhau sử dụng ký tự, ví dụ "[Việt Nam] [Vĩnh Phúc]"
        *           Nếu UniqueIndexColumns chỉ có một column thì ký tự [] bị bỏ qua, ví dụ: "Ngân hàng Á Châu"
        *       2)  Cột thứ hai là ID (kiểu long) là giá trị khóa chính của bảng con
        *  Tên của vùng tìm kiếm được tạo động = <ParentTable> + "_ID_" + <ChildTable> + "_LOOKUP"
        *  Ví dụ HU_PROVINCE_56_HU_DISTRIC_LOOKUP
        *  Cặp ParentTable ChildTable phải là đơn nhất

        *************************************************************/

        public required string ParentTable { get; set; } // ParentKey is always "ID"
        public required string ChildTable { get; set; }
        public required string ChildKey { get; set; }

        // Số lượng càng ít càng dễ nhìn
        // Danh sách đã được giới hạn theo ID của bảng cha
        // Nên thường chỉ cần một cột nào đó của bảng con là Unique Index
        public required List<string> ParentUniqueIndexColumns { get; set; }
        public required List<string> ChildUniqueIndexColumns { get; set; }
        public string? ParentCondition { get; set; } = "1=1";
        public string? ChildCondition { get; set; } = "1=1";

    }

    public class ExTable
    {
        public required string Table { get; set; }
        public string? Version { get; set; } = "1";
        public required string BufferTable { get; set; }
        public int RenderOrder { get; set; }
        public int ImportOrder { get; set; }
        public required string HeaderBgColor { get; set; }
        public required string HeaderTextColor { get; set; }
        public List<string> IdentityColumns { get; set; } = null!;
        public List<string> RenderColumns { get; set; } = null!;
        public List<string>? RequiredColumns { get; set; }
        public List<List<string>>? UniqueIndexes { get; set; }
        public required List<ExColumnRule> Rules { get; set; }
    }

    /*  Truy vấn import/export được gửi đến từ Frontend chỉ bao gồm 
    *        export interface IGenerateTemplateRequest 
    *        {`
    *          exCode: string,
    *        }
    */
    public class ExFile
    {
        public required string ExCode { get; set; }
        public int ColumnRow { get; set; } = 2;
        public int CaptionRow { get; set; } = 3;
        public int TypeRow { get; set; } = 4;
        public int DataStartRow { get; set; }
        public int RowsToPrepare { get; set; }
        public List<ExDirectReference>? ExDirectReferences { get; set; }
        public List<ExConsequentReference>? ExConsequentReferences { get; set; }
        public List<ExIndirectReference>? ExIndirectReferences { get; set; }
        public required List<ExTable> ExTables { get; set; }
        public bool? RenderOtherListReferences { get; set; }
        public bool? RenderAdministrativePlaces { get; set; }
        public bool? BuildOrgTree { get; set; }
        public bool? BuildPositions { get; set; }
    }

    public class ExObject
    {
        public string ExCode { get; set; } = null!;
        public string Lang { get; set; } = null!;

    }

    public class DownloadTemplateDTO
    {
        public required string ExCode { get; set; }
    }

    public class ImportXlsxToDbDTO
    {
        public required string FileName { get; set; }
        public required string ExCode { get; set; }
        public required string Base64String { get; set; }
    }

}
