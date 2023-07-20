using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerPlaceholder : NetworkBehaviour
{
    [SyncVar] public bool mapGenerated = false;

}
