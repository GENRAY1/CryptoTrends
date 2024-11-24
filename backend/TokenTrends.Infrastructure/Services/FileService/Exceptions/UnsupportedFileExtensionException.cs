namespace TokenTrends.Infrastructure.Services.FileService.Exceptions;

public class UnsupportedFileExtensionException(string extension) 
    : Exception($"File extension {extension} is not supported");