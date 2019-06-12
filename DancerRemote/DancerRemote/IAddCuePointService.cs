using System;
using System.Collections.Generic;
using System.Text;

namespace DancerRemote
{
    public interface IAddCuePointService
    {
        int AddCuePoint(int cuePointIndex, int cuePosition);
    }
}
