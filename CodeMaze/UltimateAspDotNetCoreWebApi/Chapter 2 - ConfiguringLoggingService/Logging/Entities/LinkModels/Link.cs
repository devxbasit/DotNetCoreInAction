namespace Entities.LinkModels;

public class Link
{
    public string? Href { get; set; }
    public string? Rel { get; set; }
    public string? Method { get; set; }

    public Link()
    {
    }

    public Link(string href, string rel, string method) =>
        (Href, Rel, Method) = (href, rel, method);
    
}