using SecureNotepadServer.Models;

namespace SecureNotepadServer.Services
{
    public interface ISecureNotepad
    {
        NotepadResponse EncodeFile(string fileName);
        PublicKey PublicKey { get; set; }
        bool IsGMAlgorithm { get; set; }
    }
}
