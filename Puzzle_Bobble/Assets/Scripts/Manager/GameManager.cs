using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // 撃った玉が移動中か
    public bool shootedBobbleMoving;

    // 泡の削除演出中か
    public bool isBobbleDeleting;

    // 泡がゲームオーバーゾーンまで落ち切ったか
    public bool isBobbleFalloutGameOverZone;
}