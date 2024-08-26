using Content.Client.UserInterface.Controls;
using Content.Shared._CD.Records;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Utility;

namespace Content.Client._CD.Records.UI;

[GenerateTypedNameReferences]
public sealed partial class RecordEntryEditPopup : FancyWindow
{
    public RecordEntryEditPopup()
    {
        RobustXamlLoader.Load(this);
        IoCManager.InjectDependencies(this);

        DescriptionEdit.Placeholder =
            new Rope.Leaf(Loc.GetString("cd-records-entry-edit-popup-description-placeholder"));

        SaveButton.OnPressed += _ =>
        {
            Close();
        };
    }

    public PlayerProvidedCharacterRecords.RecordEntry GetContents()
    {
        string desc = Rope.Collapse(DescriptionEdit.TextRope).Trim();
        return new PlayerProvidedCharacterRecords.RecordEntry(TitleEdit.Text, InvolvedEdit.Text, desc);
    }

    public void SetContents(PlayerProvidedCharacterRecords.RecordEntry entry)
    {
        TitleEdit.Text = entry.Title;
        InvolvedEdit.Text = entry.Involved;
        DescriptionEdit.TextRope = new Rope.Leaf(entry.Description);
    }
}