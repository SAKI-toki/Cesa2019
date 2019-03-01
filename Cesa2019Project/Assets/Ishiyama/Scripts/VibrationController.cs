using System.Runtime.InteropServices;

/// <summary>
/// バイブレーション
/// </summary>
public class VibrationController
{
    //DLLのインポート
    [DllImport("VibrationDll")]
    static extern void Vibration(int n, float l_power, float r_power);
    
    /// <summary>
    /// 指定したコントローラーをバイブレーションさせる関数
    /// </summary>
    /// <param name="n">コントローラーの番号</param>
    /// <param name="l_power">左のモーターの力</param>
    /// <param name="r_power">右のモーターの力</param>
    static public void XVibration(int n, float l_power, float r_power)
    {
        Vibration(n, l_power, r_power);
    }
}
