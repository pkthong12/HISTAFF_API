using API.Entities;
using CORE.Extension;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace API.Main
{
    public static class StringCodeGenerator
    {
        /// <summary>
        /// Tạo mã theo nguyên tắc tăng dần
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="suffixLength"></param>
        /// <param name="existingCodes"></param>
        /// <returns></returns>
        public static string CreateNewCode(string prefix, int suffixLength, List<string> existingCodes, string position = "LEFT")
        {
            try
            {
                // Kiểm tra các tham số đầu vào có hợp lệ hay không
                if (string.IsNullOrEmpty(prefix)) throw new ArgumentNullException("prefix");
                if (suffixLength < 1) throw new ArgumentOutOfRangeException("suffixLength");
                string newCode = "";
                int charCount = 10; //  10 chữ số
                int maxCount = (int)Math.Pow(charCount, suffixLength);
                int maxNumber = 0;
                maxNumber = existingCodes.Count == 0 ? 0 : int.Parse(existingCodes.Max().Substring(prefix.Length));
                // Lặp lại cho đến khi tìm được một mã mới không trùng với các mã đã có
                do
                {
                    string suffix = "";
                    if (position.Trim().ToUpper() == "LEFT")
                    {
                        
                        // Tăng số nguyên lên 1
                        maxNumber++;
                        // Chuyển số nguyên thành một chuỗi và thêm các số 0 vào đầu nếu cần
                        suffix = maxNumber.ToString().PadLeft(suffixLength, '0');
                        // Nối tiền tố và hậu tố lại để tạo mã mới
                        newCode = prefix + suffix;

                    }
                    else if (position.Trim().ToUpper() == "RIGHT")
                    {
                        maxNumber = existingCodes.Count == 0 ? 0 : int.Parse(existingCodes.Max().Substring(0, suffixLength));
                        // Tăng số nguyên lên 1
                        maxNumber++;
                        // Chuyển số nguyên thành một chuỗi và thêm các số 0 vào đầu nếu cần
                        suffix = maxNumber.ToString().PadLeft(suffixLength, '0');
                        // Nối tiền tố và hậu tố lại để tạo mã mới
                        newCode = suffix + prefix;
                    }
                    // Giảm số lượng mã còn lại đi 1
                    maxCount--;
                    // Kiểm tra xem số lượng mã còn lại có bằng 0 hay không, nếu có thì ném ra ngoại lệ
                    if (maxCount == 0) throw new InvalidOperationException("Không thể tạo thêm mã mới với độ dài hậu tố cho trước");
                } while (existingCodes.Contains(newCode)); // Lặp lại nếu mã mới đã tồn tại
                // Trả về mã mới
                return newCode;
            }
            catch
            {
                // return null if validation fails
                return string.Empty;
            }
        }

        public static string CreateNewCodeNonePrefix(int suffixLength, List<string> existingCodes)
        {
            try
            {
                // Kiểm tra các tham số đầu vào có hợp lệ hay không
                if (suffixLength < 1) throw new ArgumentOutOfRangeException("suffixLength");
                string newCode = "";
                int charCount = 10; //  10 chữ số
                int maxCount = (int)Math.Pow(charCount, suffixLength);
                int maxNumber = existingCodes.Count == 0 ? 0 : int.Parse(existingCodes.Max());
                // Lặp lại cho đến khi tìm được một mã mới không trùng với các mã đã có
                do
                {
                    // Tăng số nguyên lên 1
                    maxNumber++;
                    // Chuyển số nguyên thành một chuỗi và thêm các số 0 vào đầu nếu cần
                    string suffix = maxNumber.ToString().PadLeft(suffixLength, '0');
                    // Nối tiền tố và hậu tố lại để tạo mã mới
                    newCode = suffix;
                    // Giảm số lượng mã còn lại đi 1
                    maxCount--;
                    // Kiểm tra xem số lượng mã còn lại có bằng 0 hay không, nếu có thì ném ra ngoại lệ
                    if (maxCount == 0) throw new InvalidOperationException("Không thể tạo thêm mã mới với độ dài hậu tố cho trước");
                } while (existingCodes.Contains(newCode)); // Lặp lại nếu mã mới đã tồn tại
                // Trả về mã mới
                return newCode;
            }
            catch
            {
                // return null if validation fails
                return string.Empty;
            }
        }
        /// <summary>
        /// Rule: xxx/prefix + year
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="suffixLength"></param>
        /// <param name="existingCodes"></param>
        /// <returns></returns>
        public static string CreateNewCodeHD(string prefix, int suffixLength, List<string> existingCodes)
        {
            try
            {
                // Kiểm tra các tham số đầu vào có hợp lệ hay không
                if (string.IsNullOrEmpty(prefix)) throw new ArgumentNullException("prefix");
                if (suffixLength < 1) throw new ArgumentOutOfRangeException("suffixLength");
                string newCode = "";
                int charCount = 10; //  10 chữ số
                int maxCount = (int)Math.Pow(charCount, suffixLength);
                int maxNumber = existingCodes.Count == 0 ? 0 : int.Parse(existingCodes.Max().Substring(0, suffixLength));
                // Lặp lại cho đến khi tìm được một mã mới không trùng với các mã đã có
                do
                {
                    // Tăng số nguyên lên 1
                    maxNumber++;
                    // Chuyển số nguyên thành một chuỗi và thêm các số 0 vào đầu nếu cần
                    string suffix = maxNumber.ToString().PadLeft(suffixLength, '0');
                    // Nối tiền tố và hậu tố lại để tạo mã mới
                    newCode = suffix + "/" + prefix + DateTime.Now.Year.ToString();
                    // Giảm số lượng mã còn lại đi 1
                    maxCount--;
                    // Kiểm tra xem số lượng mã còn lại có bằng 0 hay không, nếu có thì ném ra ngoại lệ
                    if (maxCount == 0) throw new InvalidOperationException("Không thể tạo thêm mã mới với độ dài hậu tố cho trước");
                } while (existingCodes.Contains(newCode)); // Lặp lại nếu mã mới đã tồn tại
                // Trả về mã mới
                return newCode;
            }
            catch
            {
                // return null if validation fails
                return string.Empty;
            }
        }

        /// <summary>
        /// Tạo mã random
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="suffixLength"></param>
        /// <param name="existingCodes"></param>
        /// <returns></returns>
        public static string CreateRandomNewCode(string prefix, int suffixLength, List<string> existingCodes)
        {
            try
            {
                // Kiểm tra các tham số đầu vào có hợp lệ hay không
                if (string.IsNullOrEmpty(prefix)) throw new ArgumentNullException("prefix");
                if (suffixLength < 1) throw new ArgumentOutOfRangeException("suffixLength");
                if (existingCodes == null) throw new ArgumentNullException("existingCodes");

                // Tạo một biến để lưu trữ mã mới
                string newCode = "";

                // Tạo một biến để lưu trữ số lượng các ký tự có thể sử dụng trong hậu tố
                int charCount = 10; // Chỉ sử dụng 10 chữ số

                // Tạo một biến để lưu trữ số lượng các mã có thể tạo ra với độ dài hậu tố cho trước
                int maxCount = (int)Math.Pow(charCount, suffixLength);

                // Tạo một biến ngẫu nhiên để sinh hậu tố
                Random random = new Random();

                // Lặp lại cho đến khi tìm được một mã mới không trùng với các mã đã có
                do
                {
                    // Tạo một biến để lưu trữ hậu tố
                    StringBuilder suffix = new StringBuilder();

                    // Lặp lại cho đến khi đủ độ dài hậu tố
                    for (int i = 0; i < suffixLength; i++)
                    {
                        // Sinh một số ngẫu nhiên từ 0 đến charCount - 1
                        int index = random.Next(charCount);

                        // Chuyển số ngẫu nhiên thành một ký tự từ 0-9
                        char c = (char)('0' + index);

                        // Thêm ký tự vào hậu tố
                        suffix.Append(c);
                    }

                    // Nối tiền tố và hậu tố lại để tạo mã mới
                    newCode = prefix + suffix.ToString();

                    // Giảm số lượng mã còn lại đi 1
                    maxCount--;

                    // Kiểm tra xem số lượng mã còn lại có bằng 0 hay không, nếu có thì ném ra ngoại lệ
                    if (maxCount == 0) throw new InvalidOperationException("Không thể tạo thêm mã mới với độ dài hậu tố cho trước");

                } while (existingCodes.Contains(newCode)); // Lặp lại nếu mã mới đã tồn tại

                // Trả về mã mới
                return newCode;
            }
            catch
            {
                // return null if validation fails
                return string.Empty;
            }
        }
    }
}
