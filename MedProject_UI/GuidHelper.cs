using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedProject_UI
{
    public static class GuidHelper
    {
        public static string GenerateShortGuid()
        {
            Guid guid = Guid.NewGuid();
            string base64Guid = Convert.ToBase64String(guid.ToByteArray());

            // Replace characters to make it URL-friendly and remove trailing '=='
            string shortGuid = base64Guid.Replace("+", "-").Replace("/", "_").TrimEnd('=');

            return shortGuid;
        }
    }
}
