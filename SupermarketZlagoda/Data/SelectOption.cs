namespace SupermarketZlagoda.Data;

public class SelectOption
{
    public SelectOption()
    {
    }

    public SelectOption(string value, string text, bool selected) : this()
    {
        Value = value;
        Text = text;
        Selected = selected;
    }

    public SelectOption(string value, string text) : this()
    {
        Value = value;
        Text = text;
    }


    public string Value { get; set; }
    public string Text { get; set; }
    public bool Selected { get; set; }
}