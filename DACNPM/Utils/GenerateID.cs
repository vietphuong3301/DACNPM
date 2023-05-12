using System.Text.RegularExpressions;

namespace DACNPM.Utils
{
    public class GenerateID
    {
        public static string AutoGenerateCode(string strInput)
        {
            string strResult = "", numPart = "", strPart = "";
            numPart = Regex.Match(strInput, @"\d+").Value;
            strPart = Regex.Match(strInput, @"\D+").Value;
            int intPart = (Convert.ToInt32(numPart) + 1);
            for (int i = 0; i < numPart.Length - intPart.ToString().Length; i++)
            {
                strPart += "0";
            }
            strResult = strPart + intPart;
            return strResult;

        }
    }
}
