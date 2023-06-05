using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points
{
    public int goodGainedVP;
    public int goodLostVP;
    public bool goodHasKilledLeader;
    public int evilGainedVP;
    public int evilLostVP;
    public bool evilHasKilledLeader;

    public Points(int goodGainedVP, int goodLostVP, bool goodHasKilledLeader, int evilGainedVP, int evilLostVP, bool evilHasKilledLeader)
    {
        this.goodGainedVP = goodGainedVP;
        this.goodLostVP = goodLostVP;
        this.goodHasKilledLeader = goodHasKilledLeader;
        this.evilGainedVP = evilGainedVP;
        this.evilLostVP = evilLostVP;
        this.evilHasKilledLeader = evilHasKilledLeader;
    }
}
