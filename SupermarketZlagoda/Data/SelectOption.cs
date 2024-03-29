namespace SupermarketZlagoda.Data;

public class SelectOption
{
    public SelectOption()
    {
    }

    public SelectOption(Guid value, string text, bool selected = false) : this()
    {
        Value = value;
        Text = text;
        Selected = selected;
    }


    public Guid Value { get; set; }
    public string Text { get; set; }
    public bool Selected { get; set; }
}