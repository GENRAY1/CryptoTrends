using PetPalsProfile.Domain.Absractions;

namespace TokenTrends.Domain.Files;

public class FileErrors
{
    public static readonly Error FileNotFound =
        new ("FileErrors.FileNotFound", "File not found");
}