namespace CapturedFlag.Engine
{
    /// <summary>
    /// Add descriptions and names that can contain more specific information related to the class
    /// implementing them.
    /// </summary>
    interface IDescriptive
    {
        string Name { get; set; }
        string Description { get; set; }
    }
}