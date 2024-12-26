using PracticeCrud.Common.Helper;
using System.Data;
using System.Text.RegularExpressions;

namespace PracticeCrud.Common.Methods
{
    public class CommonMethods
    {
        public static void DatatableToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        public static void DeleteDirectory(string DirectoryPath)
        {
            string BasePath = Path.Combine(Directory.GetCurrentDirectory(), DirectoryPath);
            if (Directory.Exists(BasePath))
            {
                string[] allEntries = Directory.GetFileSystemEntries(BasePath);
                foreach (string entry in allEntries)
                {
                    if (File.Exists(entry))
                    {
                        File.Delete(entry);
                    }
                    else if (Directory.Exists(entry))
                    {
                        DeleteDirectory(entry);
                    }
                }
                Directory.Delete(BasePath);
            }
        }
        public static void DeleteFileByName(string folderPath, string fileName)
        {
            if (fileName != null)
            {
                string filePath = Path.Combine(folderPath, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        public static string[] GenerateEncryptedPasswordAndPasswordSalt(string password)
        {
            string hashed = EncryptionDecryption.Hash(EncryptionDecryption.GetEncrypt(password));
            string[] segments = hashed.Split(":");
            string EncryptedHash = EncryptionDecryption.GetEncrypt(segments[0]);
            string EncryptedSalt = EncryptionDecryption.GetEncrypt(segments[1]);
            string Hash = EncryptionDecryption.GetDecrypt(EncryptedHash);
            string Salt = EncryptionDecryption.GetDecrypt(EncryptedSalt);
            string[] EncryptedPassword = new string[4];
            EncryptedPassword[0] = EncryptedHash;
            EncryptedPassword[1] = EncryptedSalt;
            EncryptedPassword[2] = Hash;
            EncryptedPassword[3] = Salt;
            return EncryptedPassword;
        }
        public static string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandom();
            }
            return r;
        }
        public static string GenerateNewRandomVerificationCode(int length)
        {
            Random random = new Random();
            char[] chars = new char[length];
            string allowedChars = "0123456789";
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
        public static bool IsValidEmail(string email)
        {
            const string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
        public static bool IsPasswordStrong(string CreatePassword)
        {
            const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!#%*?&])[A-Za-z\d@$!#%*?&]{8,}$";
            return Regex.IsMatch(CreatePassword, pattern);
        }
        public static async Task<string> UploadImage(IFormFile File, string FilePath, string FileName, bool UseGuid)
        {
            Guid guidFile = Guid.NewGuid();
            string BasePath;
            string path;
            string Photo = string.Empty;
            if (UseGuid)
            {
                FileName = guidFile + Path.GetExtension(File.FileName);
            }
            else
            {
                FileName = FileName + Path.GetExtension(File.FileName);
            }
            BasePath = Path.Combine(Directory.GetCurrentDirectory(), FilePath);
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            path = Path.Combine(BasePath, FileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }
            Photo = FileName;
            return Photo;
        }
    }
}
