using BridgeportClaims.Word.Enums;

namespace BridgeportClaims.Word.FileDriver
{
    public interface IWordFileDriver
    {
        string GetLetterByType(LetterType type);
    }
}