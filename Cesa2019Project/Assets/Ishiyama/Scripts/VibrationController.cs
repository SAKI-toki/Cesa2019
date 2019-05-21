using System.Runtime.InteropServices;

/// <summary>
/// バイブレーション
/// </summary>
static public class XinputVibration
{
    //DLLのインポート
    [DllImport("XInputDLL")]
    static extern public void Vibration(int n, float l_power, float r_power);
    [DllImport("XInputDLL")]
    static extern public float GetTrigger(int n, bool is_right);
}
