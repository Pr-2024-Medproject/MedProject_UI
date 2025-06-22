namespace MedProject_UI;

public static class GuidHelper
{
    public static string GenerateShortGuid()
    {
        var guid = Guid.NewGuid();
        var base64Guid = Convert.ToBase64String(guid.ToByteArray());

        // Replace characters to make it URL-friendly and remove trailing '=='
        var shortGuid = base64Guid.Replace("+", "-").Replace("/", "_").TrimEnd('=');

        return shortGuid;
    }
}