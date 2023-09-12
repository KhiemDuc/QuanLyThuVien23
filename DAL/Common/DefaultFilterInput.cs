using System.Collections.Generic;

public class DefaultFilterInput : DefaultListIdFilterInput
{
    public string TextFilter { get; set; }
}
public class DefaultFilterInput<T> : DefaultListIdFilterInput<T>
{
    public string TextFilter { get; set; }
}

public class DefaultListIdFilterInput
{
    public List<int> ListId { get; set; }
    public List<int> ListExcludeId { get; set; }
}
public class DefaultListIdFilterInput<T>
{
    public List<T> ListId { get; set; }
    public List<T> ListExcludeId { get; set; }
}
