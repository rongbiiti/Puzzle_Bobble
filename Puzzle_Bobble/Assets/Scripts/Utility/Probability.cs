using UnityEngine;

public static class Probability
{
    /// <summary>
    /// �m������
    /// </summary>
    /// <param name="fPercent">�m�� (0~100)</param>
    /// <returns>���I���� [true]���I</returns>
    public static bool Lottery(float fPercent)
    {
        float fProbabilityRate = Random.value * 100.0f;

        if (fPercent == 100.0f && fProbabilityRate == fPercent)
        {
            return true;
        }
        else if (fProbabilityRate < fPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
