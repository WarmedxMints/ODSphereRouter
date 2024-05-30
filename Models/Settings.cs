namespace ODSphereRouter.Models
{
    public sealed class Settings(bool exploitedControlledOnly = true, bool autoCopyToClipboard = false, string journalPath = "", WindowPosition? position = null)
    {
        public bool ExploitedControlledOnly { get; } = exploitedControlledOnly;
        public bool AutoCopyToClipboard { get; } = autoCopyToClipboard;
        public string JournalPath { get; } = journalPath;
        public WindowPosition? Position { get; } = position;
    }
}
