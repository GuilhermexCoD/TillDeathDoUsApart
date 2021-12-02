using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent
{
    event EventHandler<EventArgs> onEndEpisode;

    void SetPositionVelocityZero(Vector3 position);
}
