using System.Collections.Generic;
using UnityEngine;

public interface ISearchingSource
{
    public Resource FindSomeResource(Instrument instrument);
    public List<DropInfo> GetDropInfo();
    public HashSet<ResourceTypeEnum?> GetFastDropInfo();
}

