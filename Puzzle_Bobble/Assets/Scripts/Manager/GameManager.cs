using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // 撃った玉が移動中か
    public bool shootedBobbleMoving;

    // 泡がゲームオーバーゾーンまで落ち切ったか
    public bool isBobbleFalloutGameOverZone;
}