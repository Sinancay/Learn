using System;
using System.Collections.Generic;

namespace RestApiHomeTask;
public partial class Item
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Categoryid { get; set; }

    public string? Details { get; set; }

    public virtual Category? Category { get; set; }
}
